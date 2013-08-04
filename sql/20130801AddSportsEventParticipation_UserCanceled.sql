-- Bart van der Wal - 1-8-2013

-- Undo queries.
-- alter table SportsEventParticipation drop column UserCanceled;
-- End of undo queries.

-- Analysis queries.
-- select * from SportsEventParticipation where UserCanceled=1;
-- End of analysis queries.

alter table SportsEventParticipation add UserCanceled bit;
