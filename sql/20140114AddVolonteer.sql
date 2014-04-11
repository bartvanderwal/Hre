-- Bart van der Wal - 14-1-2014

-- Undo queries.
-- drop table Volonteer;
-- End of undo queries.

-- Analysis queries.
select * from Volunteer v
    join LogonUser u on v.UserId = u.Id
    join Address a on a.Id = u.PrimaryAddressId
    where sportseventId=2;
-- End of analysis queries.

create table Volunteer (
  Id int NOT NULL PRIMARY KEY AUTO_INCREMENT,
  DateCreated datetime NOT NULL,
  DateUpdated datetime NOT NULL,
  DateRegistered datetime NOT NULL, -- Kan afwijken van dateCreated in geval van later verwerkte papieren inschrijvingen e.d.
  UserId int references LogonUser(Id),
  SportsEventId int references SportsEvent(Id),
  Notes varchar(1024) NULL,
  TShirtSize varchar(6) NULL,
  Food bit,
  HasBoat bit,
  PreferredTask  varchar(64) NULL
);

ALTER TABLE Volunteer
  ADD CONSTRAINT volunteer_sportevent
  FOREIGN KEY (sportsEventId)
  REFERENCES sportsevent(Id)
  ON DELETE cascade
  ON UPDATE cascade,
  ADD INDEX Volunteer(sportsEventId ASC);


ALTER TABLE Volunteer
  ADD CONSTRAINT volunteer_User
  FOREIGN KEY (UserId)
  REFERENCES logonuser(Id)
  ON DELETE cascade
  ON UPDATE cascade,
  ADD INDEX User(UserId ASC);
