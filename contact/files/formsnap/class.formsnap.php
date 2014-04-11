<?php
//=====================================================//
//
// ! YABDAB - FORMSNAP CLASS
// - edited: 09-05-2012 08:51:40 am
// - author: Mike Yrabedra
// - (c)2011 Yabdab Inc. All rights reserved.
//
//  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY 
//  EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF
//  MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL 
//  THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
//  SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT
//  OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
//  HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR 
//  TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, 
//  EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//
//=====================================================//

// ! -- LOAD REQUIRED HELPER CLASSES
require_once(dirname(__FILE__) . DIRECTORY_SEPARATOR .'class.phpmailer.php'); // phpMailer 5.2
require_once(dirname(__FILE__) . DIRECTORY_SEPARATOR .'class.smtp.php'); // phpMailer 5.2 SMTP Class
require_once(dirname(__FILE__) . DIRECTORY_SEPARATOR .'browser.php');
require_once(dirname(__FILE__) . DIRECTORY_SEPARATOR .'recaptchalib.php'); // include reCAPTCHA lib

class Formsnap {

	//  Set Variables for Class Use...
	private $mailer;
	private $recaptcha;
	private $browser;

	public $settings = array();
	public $email_content = '';
	public $receipt_content = '';
	public $files = array();
	public $errors = array();

	//======================================================================================//
	// + INSTANTIATE OTHER CLASSES
	//======================================================================================//

	function __construct() {
		$this->mailer = new phpMailer(true);//defaults to using php "mail()"; the true param means it will throw exceptions on errors, which we need to catch
		$this->browser = new Browser;			
	}	
	
	//=====================================================//
	// ! - HELPER FUNCTIONS
	//=====================================================//
	
	 public function recaptchaCheckAnswer()
	 {
	 	// Get a key from http://recaptcha.net/api/getkey
		$publickey = $this->settings['recaptcha_public_key'];
		$privatekey = $this->settings['recaptcha_private_key'];
		$resp = null;
		$error = null;
	    $resp = recaptcha_check_answer($privatekey,$_SERVER["REMOTE_ADDR"],$_POST['recaptcha_challenge_field'],$_POST['recaptcha_response_field']);
	
	    if (!$resp->is_valid) {
	            # set the error code so that we can display it
	            return $resp->error;				
	    }
		
		return 'good';
		
	}
	
	public function processForm()
	{
		
		if(!$this->settings['do_not_send'])
		{
			// send main message
			$this->buildEmailContent();
			$this->sendMessage();
			
			// send receipt to submitter
			if($this->settings['send_receipt'])
			{
				$this->buildReceiptContent();
				$this->sendReceipt();
			}
		}
		
		// Save to My SQL?
		if($this->settings['save_to_mysql']){
			$rs = $this->saveToMySQL();
			if($rs != 'good')
				@array_push($this->errors, $rs);
			unset($rs);
		}
				
		
	}
	
	//=====================================================//
	// processAttachments
	//=====================================================//
	
	public function processAttachments()
	{
		if (isset($_FILES)) {

		foreach ($_FILES as $attachment) {
		
			$attachment_name = $attachment['name'];
			$attachment_size = $attachment["size"];
		  	$attachment_temp = $attachment["tmp_name"];
		  	$attachment_type = $attachment["type"];
		  	$attachment_ext  = explode('.', $attachment_name);  
		  	$attachment_ext  = $attachment_ext[count($attachment_ext)-1]; 
		  	
		  	if ( $attachment['error'] === UPLOAD_ERR_OK && is_uploaded_file($attachment['tmp_name']) ) {
		
		  		// User set filtering; size, ext, etc...
		  		if(stristr($this->settings['allowed_file_types'], $attachment_ext) == FALSE){ // make sure we accet this file type....
		  			return $this->settings['file_not_allowed'];
		  		} 
		  		
		  		// See is file is not too big.
				if($attachment_size > $this->settings['max_attachment_size_bytes']){
					return $this->settings['file_too_big'] ;
				}
						
				$file = fopen($attachment_temp,'rb');
				$data = fread($file,filesize($attachment_temp));
				fclose($file);
				$data = chunk_split(base64_encode($data));
		        
		                  
			// add to array for phpmailer to use...
			array_push($this->files, array('temp'=> $attachment_temp, 'name' => $attachment_name));
		  	
		  	} else if ($attachment['error'] !== UPLOAD_ERR_NO_FILE) {
		  	
		  		// ! -- SERVER ERRORS
				switch ($attachment['error']) {
					case UPLOAD_ERR_INI_SIZE:
					case UPLOAD_ERR_FORM_SIZE:
						$error = "File $attachment_name exceeds the " . ini_get('upload_max_filesize') . 'B limit for the server.';
						break;
					case UPLOAD_ERR_PARTIAL:
						$error = "Only part of the file $attachment_name could be uploaded, please try again.";
						break;
					default:
						$error = "There has been an error attaching the file $attachment_name, please try again.";
				}
				
				return $error;
		  	
		  	} // file was uploaded
		  	
		  }
		  
		  	
		}
		
		return 'good';
	
	}

	
	
	private function saveToMySQL()
	{
		
		// Connect to Database
		$s = @mysql_connect($this->settings['mysql_host'], $this->settings['mysql_user'], $this->settings['mysql_password']);
		@mysql_query("SET NAMES 'utf8'"); //this is new
		@mysql_query("SET CHARACTER SET 'utf8'"); //this is new
		$d = @mysql_select_db($this->settings['mysql_db'], $s);
		
			
		if(!$s || !$d){ 		
			return "Cannot connect to database, ".$this->settings['mysql_db'];	
		}
		
		// set encoding to utf-8
		mysql_set_charset('utf8');
		
		$inserts = '';
		foreach ($_POST['form'] as $key => $value) 
		{
			$savetomysql_key = $key.'_savetomysql';
			if(isset($_POST[$savetomysql_key]) && !empty($_POST['form'][$key]) )
			{
				$v = $_POST['form'][$key];
				$v = $this->value($v, false);
				$inserts .= $_POST[$savetomysql_key]." = '" . mysql_real_escape_string($v) . "',";
				unset($v);
			}
		}
		
		$inserts = substr($inserts, 0, -1); // remove last comma
		
		// Save data
		$sql = 'INSERT INTO `'. $this->settings['mysql_db']. '`.`'. $this->settings['mysql_table']. '` SET '. $inserts;
		
		$rs = @mysql_query($sql);
		unset($sql);
		
		// Add returning MySQL errors
		
		if( mysql_errno() ){ 		
			$mysql_error= "MySQL Error: (".mysql_errno().") ".mysql_error()."<br />When executing:<br />".$mySQLQuery;
			return  $mysql_error;	
		}
	
		return 'good';
	
	
	}
	
	private function sendMessage()
	{
	
		// Clear all addresses and attachments for next loop
		$this->mailer->ClearAllRecipients();
		$this->mailer->ClearAttachments();
		
		$this->mailer->CharSet = $this->settings['encoding_charset'];
		$this->mailer->Encoding = $this->settings['encoding_method'];
		
		if($this->settings['use_smtp'])
				$this->mailer->IsSMTP(); // telling the class to use
		
		try {
		
			if($this->settings['use_smtp']){
	
				$this->mailer->Host       = $this->settings['smtp_host'];	// SMTP server
				$this->mailer->SMTPDebug  = $this->settings['smtp_debug'];	// enables SMTP debug information (for testing)(1,2)
														// 1 = errors and messages
														// 2 = messages only
				if($this->settings['smtp_secure'])
					$mail->SMTPSecure = $this->settings['smtp_secure_prefix'];
					
				$this->mailer->SMTPAuth   = $this->settings['smtp_auth'];	// enable SMTP authentication (bool)
				$this->mailer->Port       = $this->settings['smtp_port'];	// set the SMTP port
				$this->mailer->Username   = $this->settings['smtp_username']; // SMTP account username
				$this->mailer->Password   = $this->settings['smtp_password'];	// SMTP account password
			}
		
		// To recipients...
		$to = explode(',', $this->settings['to']);
		foreach($to as $to_email) {
			$this->mailer->AddAddress(trim($to_email));
		}
		
		// Cc recipients...
		if($this->settings['cc']) {
			$cc = explode(',', $this->settings['cc']);
			foreach($cc as $cc_email) {
				$this->mailer->AddCC(trim($cc_email));
			}
		}
	
		// Bcc recipients...
		if($this->settings['bcc']) {
			$bcc = explode(',', $this->settings['bcc']);
			foreach($bcc as $bcc_email) {
				$this->mailer->AddBCC(trim($bcc_email));
			}
		}

		  $this->mailer->AddReplyTo($_POST['form'][$this->settings['reply_to_item']]);
		  $this->mailer->SetFrom($this->settings['from_email'], $this->settings['from_name']);
		  $this->mailer->Subject = strip_tags($this->settings['subject']);
		  //$this->mailer->AltBody = $this->email_text_content; // optional - MsgHTML will create an alternate automatically
		  $this->mailer->MsgHTML($this->email_content);
		  
		  if($this->files){
				for ( $i = 0; $i < sizeof ( $this->files ); $i++ )
				{
					$this->mailer->AddAttachment($this->files[$i]['temp'], $this->files[$i]['name']);   
				}
			}
	
		  
		  
		  $this->mailer->Send();
		 
		} catch (phpmailerException $e) {
			return $e->errorMessage();
		} catch (Exception $e) {
			return  $e->getMessage();
		}
	
		return 'good';
	
	}
	
	private function sendReceipt()
	{
	
		// Clear all addresses and attachments for next loop
		$this->mailer->ClearAllRecipients();
		$this->mailer->ClearAttachments();
		
		$this->mailer->CharSet = $this->settings['encoding_charset'];
		$this->mailer->Encoding = $this->settings['encoding_method'];
		
		if($this->settings['use_smtp'])
				$this->mailer->IsSMTP(); // telling the class to use
		
		try {
		
			if($this->settings['use_smtp']){
	
				$this->mailer->Host       = $this->settings['smtp_host'];	// SMTP server
				$this->mailer->SMTPDebug  = $this->settings['smtp_debug'];	// enables SMTP debug information (for testing)(1,2)
														// 1 = errors and messages
														// 2 = messages only
				if($this->settings['smtp_secure'])
					$mail->SMTPSecure = $this->settings['smtp_secure_prefix'];
					
				$this->mailer->SMTPAuth   = $this->settings['smtp_auth'];	// enable SMTP authentication (bool)
				$this->mailer->Port       = $this->settings['smtp_port'];	// set the SMTP port
				$this->mailer->Username   = $this->settings['smtp_username']; // SMTP account username
				$this->mailer->Password   = $this->settings['smtp_password'];	// SMTP account password
			}
		
		// To recipients...
		$this->mailer->AddAddress($_POST['form'][$this->settings['reply_to_item']]);
	
	
		$this->mailer->AddReplyTo($this->settings['from_email'], $this->settings['from_name']);
		$this->mailer->SetFrom($this->settings['from_email'], $this->settings['from_name']);
		$this->mailer->Subject = strip_tags($this->settings['receipt_prefix'].$this->settings['subject']);
		$this->mailer->MsgHTML($this->receipt_content);
		  
		  if($files)
		  {
				for ( $i = 0; $i < sizeof ( $files ); $i++ )
				{
					$this->mailer->AddAttachment($files[$i]['temp'], $files[$i]['name']);   
				}
			}

	  
	  
	  $this->mailer->Send();
		 
		} catch (phpmailerException $e) {
			return 'r:'.$e->errorMessage();
		} catch (Exception $e) {
			return  'r:'.$e->getMessage();
		}
		
		return 'good';
	
	
	}
	
	private function buildEmailContent()
	{
	
		if(!isset($this->settings['email_template']) || empty($this->settings['email_template']) )
		{
			
			$this->email_content = $this->buildDefaultHtmlContent();
		
		}else{
		
			$this->email_content = $this->parseTemplate('email');
		
		}
	
	
	}
	
	private function buildReceiptContent()
	{
	
		if(!isset($this->settings['receipt_template']) || empty($this->settings['receipt_template']) )
		{
			$this->receipt_content = $this->buildDefaultHtmlContent('receipt');
		}else{
			$this->receipt_content = $this->parseTemplate('receipt');
		}
	
	}
	
	//=====================================================//
	// buildDefaultHtmlContent
	//=====================================================//
	
	private function buildDefaultHtmlContent($type='email')
	{
	
		$subject = ($type == 'receipt')	? $this->settings['receipt_prefix'].$this->settings['subject'] : $this->settings['subject'];
			
		//Set a variable for the message content
		$str = "<html>".PHP_EOL."<head>".PHP_EOL."<title>" .
		                $this->safeEscapeString($subject) .
		                "</title>".PHP_EOL."</head>".PHP_EOL."<body>".PHP_EOL."<p>".PHP_EOL;
		                
		$str .= "<table><tbody>".PHP_EOL;
		
		if($type == 'receipt')
		{
			$str .= '<p>We received your message containing the following values...</p>';
		}
	
		//build a message from the reply for both HTML and text in one loop.
		foreach ($_POST['form'] as $key => $value) {
						
				$str .= '<tr><td align="right" valign="top" nowrap><b>' . strip_tags($key) . '</b></td><td> ';
				$str .= nl2br($this->value($value)) . "</td></tr>".PHP_EOL;
			
		}
		//close the HTML content.
		$str .= "</tbody></table>".PHP_EOL;
		$str .= "</p>".PHP_EOL."</body>".PHP_EOL."</html>";
		
		return $str;
	
	}
	 
	//=====================================================//
	// parseTemplate
	//=====================================================//
	
	private function parseTemplate($type = 'email')
	{
	
		$str = ($type == 'email') ? $this->settings['email_template'] : $this->settings['receipt_template'];
			
		foreach ($_POST['form'] as $key => $value) 
		{
			$str = str_replace("#{$key}#", nl2br($this->value($value)), $str);
		}
		
		// browser
		$agent = $this->browser->getBrowser().' v.'.$this->browser->getVersion(). ' ('.$this->browser->getPlatform().')';
		$str = str_replace("#browser#", $agent, $str);
		// ip address
		$str = str_replace("#ip_address#", $_SERVER['REMOTE_ADDR'], $str);
		// date
		$str = str_replace("#date#", date('r'), $str);
		
		return $str;
	
	}
	
	//=====================================================//
	// getOutput
	//=====================================================//
	
	public function getOutput($errors, $required) {
	
		if(count($errors)){
		 	$missing = array();
		 	$msg = '<h3>'.$this->settings['error_message'].'</h3>';
			$msg .= '<ul>';
			foreach($errors as $error)
				$msg .= '<li>'.$error.'</li>';
			$msg .= '</ul>';
			
			if($this->settings['error_redirect'])
			{
				//return '{ "result" : "fail", "msg" : "<h3>'.$this->settings['redirect_message'].'</h3>" }';
				return json_encode( array('result'=>'fail', 'msg'=>htmlspecialchars('<h3>'.$this->settings['redirect_message'].'</h3>') ) );
			}
			
			if(isset($required) && is_array($required))
				{		
					$missing = $required;
				}
			//return '{"result" : "fail", "msg" : "'.$msg.'", "required": '.$missing.'}';
			return json_encode( array('result'=>'fail', 'msg'=>htmlspecialchars($msg), 'required'=>$missing ) );
			unset($msg);
			unset($missing);
		}else{
			if($this->settings['success_redirect'])
			{
				//return '{ "result" : "good", "msg" : "<h3>'.$this->settings['redirect_message'].'</h3>" }';
				return json_encode( array('result'=>'good', 'msg'=>htmlspecialchars('<h3>'.$this->settings['redirect_message'].'</h3>') ) );
			}else{
				//return '{ "result" : "good", "msg" : "<h3>'.$this->settings['success_message'].'</h3>" }';
				return json_encode( array('result'=>'good', 'msg'=>htmlspecialchars('<h3>'.$this->settings['success_message'].'</h3>') ) );
			}
		}
		
		return;
	
	
	}
	 

	//=====================================================//
	// validateEmail
	//=====================================================//
	
	public function validateEmail($email) {
	   //check for all the non-printable codes in the standard ASCII set,
	   //including null bytes and newlines, and exit immediately if any are found.
	   if (preg_match("/[\\000-\\037]/",$email)) {
	      return false;
	   }
	   $pattern = "/^[-_a-z0-9\'+*$^&%=~!?{}]++(?:\.[-_a-z0-9\'+*$^&%=~!?{}]+)*+@(?:(?![-.])[-a-z0-9.]+(?<![-.])\.[a-z]{2,6}|\d{1,3}(?:\.\d{1,3}){3})(?::\d++)?$/iD";
	   if(!preg_match($pattern, $email)){
	      return false;
	   }
	   return true;
	} 
	
	//=====================================================//
	// checkRequiredFields
	//=====================================================//
	
	// Function to check the required fields are filled in.
	public function checkRequiredFields() {
			
		$err = 0;
		$fields = array();
		$rf = array(); // required fields array

		// loops through all post args and look for required args...
		foreach($_POST as $key => $value) {
			if(preg_match('/_required/', $key, $matches ))
			{
				array_push($rf, str_replace('_required', '', $key));
			}
		}
			
		foreach($rf as $key) {
		
			if (isset($_FILES[$key])) {
				$string = (isset($_FILES[$key]['name'])) ? $_FILES[$key]['name'] : '';
			} else {			
				$string = $this->value($_POST['form'][$key]);
			}
						
			if( empty($string) )
			{
				//echo "key:{$key} ( {$_POST[$required_key]} )<br />";
				array_push($fields, $key);
				$err++;
			}
			
			unset($v);
			
		}
		
		if($err){
			return $fields;
		}else{
			return 'good';
		}
	}
	

	//=====================================================//
	// value
	//=====================================================//
	
	public function value($var, $encode=true){
	
		$sep=", ";
		
		if($encode)
			$var = $this->htmlEntities($var); // encode all html tags

		if(is_array($var)){ 
			$str = (isset($var)) ? implode($sep, $var) : '';
		}else{
			$str = (isset($var)) ? $var : '';
		}
		return $str;
	}
	
	//=====================================================//
	// stringCleaner
	//=====================================================//
	
	private function stringCleaner($str) { 
		$str = preg_replace( '((?:\n|\r|\t|%0A|%0D|%08|%09)+)i' , '', $str );
		// Remove those slashes
		if(get_magic_quotes_gpc()){
			$str = stripslashes($str);
		}
		return $str;
	}
	
	//=====================================================//
	// safeEscapeString
	//=====================================================//
	
	// Function to escape data inputted from users. This is to protect against embedding
	// of malicious code being inserted into the HTML email.
	
	private function safeEscapeString($string) {
		$str = strip_tags($string);
		$str = $this->htmlEntities($str);
		return $str;
	}
	
	//=====================================================//
	// htmlEntities
	//=====================================================//
	
	private function htmlEntities($mixed, $quote_style = ENT_QUOTES, $charset = 'UTF-8') 
	{ 
	    if (is_array($mixed)) { 
	        foreach($mixed as $key => $value) { 
	            $mixed[$key] = $this->htmlEntities($value, $quote_style, $charset); 
	        } 
	    } elseif (is_string($mixed)) { 
	        $mixed = htmlentities(html_entity_decode($mixed, $quote_style), $quote_style, $charset); 
	    } 
	    return $mixed; 
	}
	
	//=====================================================//
	// html2Text
	//=====================================================//
	
	private function html2Text($str) 
	{ 
	    $text_str = str_replace("<br />", PHP_EOL, $str); // convert breaks into line-breaks.
	    $text_str = strip_tags($text_str); // remove all markup.
	    $text_str = htmlspecialchars_decode($text_str); // convert values with special chars back to unencoded.
	    return $text_str;
	}
	
	

}

//=====================================================//
// !  THE END 
//=====================================================//