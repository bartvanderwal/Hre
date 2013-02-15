-- SET PASSWORD FOR root@localhost=PASSWORD('newpass');

-- Execute this file with:
-- mysql hre -uroot -pHelen create_hre.sql

-- GRANT ALL PRIVILEGES ON *.* TO 'root'@'localhost' IDENTIFIED BY 'Helen' WITH GRANT OPTION;

create database if not exists hre;
use hre;

-- An address, the primary addres, or possibly secondary addresses linked to the user.
create table Address (
  Id int NOT NULL PRIMARY KEY AUTO_INCREMENT,
  Firstname varchar(64),
  Lastname varchar(64),
  Insertion varchar(20),
  CompanyName varchar(64),
  Street varchar(64),
  Housenumber varchar(20),
  HouseNumberAddition varchar(20),
  PostalCode varchar(20),
  City varchar(64),
  Country varchar(64),
  DateCreated datetime NOT NULL,
  DateUpdated datetime NOT NULL
);

-- The logon user with his information.
create table LogonUser (
  Id int NOT NULL PRIMARY KEY AUTO_INCREMENT,
  ExternalId varchar(64),
  EmailAddress varchar(64),
  TelephoneNumber varchar(16),
  IsActive bit,
  IsMailingListMember bit,
  StatusId int,
  EntryKey varchar(64),
  Name varchar(64),
  Language varchar(64),
  Gender bit,
  DateOfBirth datetime,
  PrimaryAddressId int references Address(Id),
  NtbLicenseNumber varchar(16),
  DateCreated datetime NOT NULL,
  DateUpdated datetime NOT NULL,
  UserName varchar(64) NOT NULL
);

create table SportsEvent (
  Id  int NOT NULL PRIMARY KEY AUTO_INCREMENT,
  Name varchar(64),
  EventDate datetime NULL,
  EventPlace varchar(64) NULL,
  DateCreated datetime NOT NULL,
  DateUpdated datetime NOT NULL,
  ExternalEventIdentifier varchar(64),
  ExternalEventSerieIdentifier varchar(64)
);

create table SportsEventParticipation (
  Id int NOT NULL PRIMARY KEY AUTO_INCREMENT,
  DateCreated datetime NOT NULL,
  DateUpdated datetime NOT NULL,
  DateRegistered datetime NOT NULL, -- Kan afwijken van dateCreated in geval van later verwerkte papieren inschrijvingen e.d.
  DateFirstScraped datetime NULL,
  DateLastScraped datetime NULL,
  DatePaymentReceived datetime NULL, -- Datum van (laatste) rekeningafschrift. Dit is de effectieve inschrijfdatum.
  UserId int references LogonUser(Id),
  SportsEventId int references SportsEvent(Id),
  MyLapsChipIdentifier varchar(64) NULL,
  Notes varchar(1024) NULL,
  SpeakerRemarks varchar(1024) NULL,
  OrganisationRemarks varchar(1024) NULL,
  ParticipationAmountInEuroCents int NULL, --  Te betalen inschrijfgeld (afhankelijk van NTB lidmaatschap, eigen chip of niet en tijdstip van betalen ivm tussentijdse prijsverhogingen.
  ParticipationAmountPaidInEuroCents int NULL, -- Totale betaalde bedrag. Zou gelijk moeten zijn aan te bet
  TShirtSize varchar(6) NULL,
  Food bit,
  Camp bit,
  Bike bit,
  HasPaid bit,
  StartNumber int NULL,
  PaymentType int NULL,
  ParticipationStatus int NULL,
  ExternalIdentifier varchar(45) NULL,
  YouTubeVideoCode varchar(64),
  Source int NULL,
  EarlyBird bit,
  FreeStarter bit
);


-- Secondary addresses, for instance for friends, clients etc.
create Table UserSecondaryAddress (
  Id int NOT NULL PRIMARY KEY AUTO_INCREMENT,
  AddressTypeId int,
  UserId int references LogonUser (Id),
  AddressId int references Address (Id),
  DateCreated datetime NOT NULL,
  DateUpdated datetime NOT NULL
);


create table EmailAudit (
  Id int NOT NULL PRIMARY KEY AUTO_INCREMENT,
  ToAddresses varchar(1024),
  FromAddress varchar(1024) NOT NULL,
  CCAddresses varchar(1024),
  BccAddresses varchar(1024),
  Subject varchar(1024),
  Body text,
  isHtml bit,
  AttachmentEntityId int,
  EmailStatusId int NOT NULL,
  StatusMessage varchar(1024),
  UserIdSender int,
  UserIdReceiver int,
  DateCreated datetime NOT NULL,
  DateUpdated datetime NOT NULL,
  DateSent datetime,
  EmailCategoryId int
);


create table PaymentType (
  Id int NOT NULL PRIMARY KEY,
  ExternalId varchar(80) NOT NULL,
  Name varchar(80) NOT NULL
);


create table Newsletter (
  Id int NOT NULL PRIMARY KEY AUTO_INCREMENT,
  Title varchar(256),
  CultureId int NOT NULL,
  DateSent datetime,
  SequenceNumber int NOT NULL,
  AddPersonalLoginLink bit,
  Audience int,
  DateCreated datetime NOT NULL,
  DateUpdated datetime NOT NULL
);

create table NewsletterItem (
  Id int NOT NULL PRIMARY KEY AUTO_INCREMENT,
  NewsletterId int NOT NULL,
  ItemTitle varchar(256),
  ItemSubTitle varchar(256),
  ItemText text,
  PictureURL varchar(256),
  IconPictureURL varchar(256),
  HeadingHtmlColour varchar(8),
  SequenceNumber int NOT NULL
);

ALTER TABLE EmailAudit ADD CONSTRAINT UserIdReceiver
  FOREIGN KEY (UserIdReceiver)
  REFERENCES logonuser(Id)
  ON DELETE CASCADE
  ON UPDATE CASCADE
  , ADD INDEX UserIdReceiver(UserIdReceiver ASC);


ALTER TABLE EmailAudit
  ADD CONSTRAINT UserIdSender
  FOREIGN KEY (UserIdSender)
  REFERENCES logonuser(Id)
  ON DELETE CASCADE
  ON UPDATE CASCADE
  , ADD INDEX UserIdSender(UserIdSender ASC);


ALTER TABLE NewsletterItem
  ADD CONSTRAINT Newsletter
  FOREIGN KEY (NewsletterId)
  REFERENCES newsletter(Id) 
  ON DELETE CASCADE
  ON UPDATE CASCADE
  , ADD INDEX Newsletter(NewsLetterId ASC);


-- Foreign keys
ALTER TABLE SportsEventparticipation
  ADD CONSTRAINT sportevent
  FOREIGN KEY (sportsEventId)
  REFERENCES sportsevent(Id)
  ON DELETE cascade
  ON UPDATE cascade,
  ADD INDEX sportsevent(sportsEventId ASC);


ALTER TABLE SportsEventParticipation
  ADD CONSTRAINT User
  FOREIGN KEY (UserId)
  REFERENCES logonuser(Id)
  ON DELETE cascade
  ON UPDATE cascade
  , ADD INDEX User(UserId ASC);


ALTER TABLE LogonUser
  ADD CONSTRAINT PrimaryAddress
  FOREIGN KEY (PrimaryAddressId)
  REFERENCES address(Id)
  ON DELETE cascade
  ON UPDATE cascade
  , ADD INDEX PrimaryAddress(PrimaryAddressId ASC);

alter table SportsEventParticipation 
    add constraint uc_UserId unique (UserId, SportsEventId);