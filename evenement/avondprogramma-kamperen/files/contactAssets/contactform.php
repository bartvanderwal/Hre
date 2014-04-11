<?php

 		function utf8_urldecode($str) { $str = preg_replace("/%u([0-9a-f]{3,4})/i","&#x\\1;",urldecode($str)); return html_entity_decode($str,null,'=?UTF-8?B?');; } 

		function mail_utf8($to, $subject = '(No subject)', $message, $headers) {
		$header_ = 'MIME-Version: 1.0' . "\r\n" . 'Content-type: text/plain; charset=UTF-8' . "\r\n";
		return mail($to, '=?UTF-8?B?'.base64_encode($subject).'?=', $message, $header_ . $headers);
		}
		
		function mail_utf8html($to, $subject = '(No subject)', $message, $headers) {
		$header_ = 'MIME-Version: 1.0' . "\r\n" . 'Content-type: text/html; charset=UTF-8' . "\r\n";
		return mail($to, '=?UTF-8?B?'.base64_encode($subject).'?=', $message, $header_ . $headers);
		}


    $emaillabel = strip_tags($_POST['emaillabel']);
    $namelabel = strip_tags($_POST['namelabel']);
    $subjectlabel = strip_tags($_POST['subjectlabel']);
    $messagelabel = strip_tags($_POST['messagelabel']);
    
    $mail = $_POST['mail'];
    $name = strip_tags($_POST['name']);
    $subject = strip_tags($_POST['subject']);
    $text = strip_tags($_POST['text']);
    $text = utf8_urldecode($text);
    $text = stripslashes($text);
    
    $owneremail = $_POST['owneremail'];
	$cleanowneremail = substr($owneremail, 7); // removes the word "obscure" from the front of the posted owner email address

	
	

    $recipientEmail = explode(',', $cleanowneremail);
    foreach($recipientEmail as $value) {
	$to = trim($value);
	$subject = stripslashes($subject);
	$message = trim($emaillabel)." : ".trim($mail)."\n".trim($namelabel)." : ".trim($name)."\n".trim($subjectlabel)." : ".trim($subject)."\n ".trim($messagelabel)." : ".trim($text);
	$message = stripslashes($message);
	
	$headers = "From: " . $cleanowneremail . "\n";
	$headers .= "Reply-To: ". $mail . "\n";
    if(mail_utf8($to,$subject,$message,$headers)){
    echo "success";
    }
    else{
    echo "error";
    }
	    }


	

	
	$autoreply = utf8_urldecode($_POST['autoreply']);
	$autoreplysig = $_POST['autoreplysig']; // if auto reply is selected in the stack
	$autoreplysub = $_POST['autosubject'];
	if($autoreply != "noreply"){
	$autoreplysig = utf8_urldecode($autoreplysig);
	$autoreply = $autoreply."<br /><br />".$autoreplysig;
	$autoreply .= "<br /><br />Your Message : ".$text;
	$autoreplysubject = utf8_urldecode($autoreplysub);
    $headers = "From: " . $cleanowneremail . "\r\n";
    $headers .= "Reply-To: ". $cleanowneremail . "\r\n";
    mail_utf8html($mail,$autoreplysubject,$autoreply,$headers);
    }
    
?>