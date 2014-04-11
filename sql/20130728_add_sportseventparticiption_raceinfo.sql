-- Bart van der Wal - 27-07-2013:
-- Add columns with race info like start number, start time, category, and result times both overall and for sections.

-- Undo queries.
-- alter table SportsEventParticipation drop column RaceNumber, drop column PlannedStartTime, drop column RaceCat, drop column WantsToDoFinal, drop column ResultPosition, drop column ResultPositionInCategory, drop column TotalTime, drop column TimeInFinal, drop column TimeOnTimedSwimPart, drop column TimeOnTimedRunPart, drop column VirtualRegistrationDateForOrdering, drop column HasPaidEnoughToList;
-- End of undo queries. 

-- Add columns for making startlist and loggin results of races (to add 2013 time, and show 2012 time as well).
alter table SportsEventParticipation add VirtualRegistrationDateForOrdering DateTime;
alter table SportsEventParticipation add RaceNumber int;
alter table SportsEventParticipation add PlannedStartTime Time;
alter table SportsEventParticipation add RaceCat varchar(3);
alter table SportsEventParticipation add WantsToDoFinal bit;
alter table SportsEventParticipation add ResultPosition int;
alter table SportsEventParticipation add ResultPositionInCategory int;
alter table SportsEventParticipation add TotalTime Time;
alter table SportsEventParticipation add TimeInFinal Time;
alter table SportsEventParticipation add TimeOnTimedSwimPart Time;
alter table SportsEventParticipation add TimeOnTimedRunPart Time;

alter table SportsEventParticipation add HasPaidEnoughToList bit;

update SportsEventParticipation set HasPaidEnoughToList=1 where (ParticipationAmountInEuroCents>=2000 or FreeStarter=1) and SportsEventId=2;
update SportsEventParticipation set HasPaidEnoughToList=0 where ParticipationAmountInEuroCents<2000 and (FreeStarter is null or FreeStarter=0) and SportsEventId=2;
