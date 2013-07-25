-- Undo queries.
-- drop table payment;
-- update sportseventparticipation set ParticipationAmountInEuroCents=null where ParticipationAmountInEuroCents is not null and sportseventId=2;
-- End of undo queries.

-- Analysis queries.
-- Dd 22-7-2013: 514 inschrijvingen, 499 zonder bedrag door de fout, totaal 124 Early Birds.
-- Er zijn 214 met eigen MyLaps chip (andere 300 moeten 2,- betalen) en 242 met NTB licentie, waarvan 198 met een atleten of aspiranten licentie (e.g. 44+272=316 moeten daglicentie kopen)
-- en 392 deelnemers hebben hun e-mail adres al/pas bevestigd.
-- select * from sportseventparticipation where SportsEventId=2;
-- select * from sportseventparticipation where ParticipationAmountInEuroCents is null and SportsEventId=2;
-- select * from sportseventparticipation where FreeStarter=1 and SportsEventId=2;
-- select * from sportseventparticipation where EarlyBird=1 and SportsEventId=2;
-- select * from sportseventparticipation where MyLapsChipIdentifier is not null and MyLapsChipIdentifier <> '' and SportsEventId=2;
-- select * from sportseventparticipation p join LogonUser u on u.Id= p.UserId where u.NtbLicenseNumber is not null and SportsEventId=2 and (u.NtbLicenseNumber like '__A%' or u.NtbLicenseNumber like '__X%');
-- select * from sportseventparticipation p  join LogonUser u on u.Id= p.UserId where u.StatusId = 2 and SportsEventId=2;

select * from sportseventparticipation where ParticipationAmountInEuroCents=ParticipationAmountPaidInEuroCents;
select * from sportseventparticipation where ParticipationAmountInEuroCents is null or ParticipationAmountPaidInEuroCents = 0;
select * from sportseventparticipation where ParticipationAmountInEuroCents>0 and ParticipationAmountInEuroCents<>ParticipationAmountPaidInEuroCents;


-- Interesses: 99 fietsen, 286 eten, 68 kamperen.
-- select * from sportseventparticipation where Bike=1 and SportsEventId=2;
-- select * from sportseventparticipation where Food=1 and SportsEventId=2;
-- select * from sportseventparticipation where Camp=1 and SportsEventId=2;
-- End of analysis queries.

-- Set the payment amounts. These were reset by a faulty script yesterday.
-- Participation amount: 27,50
-- Early bird reduction: 7,50 (so 20,- participation amount)
-- Free starters pay nothing: 0,- (so 27,50 participation amount) (Rob Barel+Vrouw Cora Mulder +Dennis Rijnbeek+4 junioren Sander)
-- Mylaps chip +2,- (so 0,- when Chipcode filled in)
-- NTB license: +2,30

-- select * from LogonUser where EmailAddress like '%hcc%' or EmailAddress like '%denrijn%';
-- select * from SportsEventParticipation p join logonUser u on u.Id = p.UserId where UserId in (72, 330, 487) and SportsEventId=2;
update SportsEventParticipation set FreeStarter=1 where UserId in (72, 330, 487) and SportsEventId=2;
--
update sportseventparticipation set ParticipationAmountInEuroCents=2750 where SportsEventId=2;
update sportseventparticipation set ParticipationAmountInEuroCents=2000 where EarlyBird=1 and sportsEventId=2;
update sportseventparticipation set ParticipationAmountInEuroCents=0 where FreeStarter=1 and sportsEventId=2;

update sportseventparticipation set participationAmountInEuroCents=participationAmountInEuroCents+200 where sportsEventId=2 and
    -- Id not in (select Id from sportseventparticipation where MyLapsChipIdentifier is not null and MyLapsChipIdentifier <> '' and SportsEventId=2);
	Id not in (361, 362, 363, 365, 366, 367, 368, 369, 374, 375, 377, 380, 381, 383, 390, 391, 392, 393, 395, 396, 400, 401, 404, 405, 407, 408, 411, 412, 414, 416, 418, 421, 422, 424, 426, 429, 430, 431, 434, 437, 438, 439, 440, 441, 442, 444, 445, 447, 449, 456, 457, 458, 461, 462, 463, 466, 471, 480, 481, 482, 483, 485, 486, 498, 499, 500, 504, 506, 507, 511, 512, 513, 514, 515, 516, 518, 522, 523, 524, 526, 527, 528, 531, 532, 538, 543, 546, 552, 553, 557, 558, 566, 570, 576, 577, 578, 580, 581, 582, 588, 589, 590, 592, 596, 599, 600, 602, 604, 605, 610, 611, 615, 621, 625, 626, 627, 628, 629, 632, 633, 634, 635, 637, 638, 640, 643, 644, 648, 650, 658, 661, 667, 668, 670, 684, 685, 687, 691, 693, 694, 700, 701, 702, 705, 707, 708, 712, 715, 716, 717, 720, 721, 724, 726, 727, 729, 730, 735, 736, 740, 742, 745, 747, 749, 750, 756, 757, 765, 777, 780, 781, 788, 795, 796, 798, 804, 807, 808, 809, 810, 812, 814, 816, 821, 824, 827, 828, 832, 835, 838, 840, 842, 843, 847, 850, 852, 853, 855, 857, 858, 859, 865, 866, 871, 872, 877, 880, 881, 882, 883, 884, 885, 886, 887);

update SportsEventParticipation set ParticipationAmountInEuroCents=ParticipationAmountInEuroCents+220 where sportsEventId=2 and
    -- Id not in (select p.Id from sportseventparticipation p join LogonUser u on u.Id= p.UserId where u.NtbLicenseNumber is not null and (u.NtbLicenseNumber like '__A%' or u.NtbLicenseNumber like '__X%') and SportsEventId=2);
	Id not in (362, 365, 366, 368, 369, 370, 372, 373, 375, 377, 381, 382, 383, 391, 392, 395, 400, 404, 405, 407, 408, 410, 412, 414, 416, 417, 418, 419, 422, 430, 431, 432, 437, 441, 444, 445, 446, 447, 449, 453, 456, 458, 459, 461, 463, 464, 465, 467, 468, 470, 471, 476, 477, 481, 482, 483, 486, 487, 489, 498, 499, 500, 504, 506, 507, 511, 513, 514, 516, 518, 519, 522, 523, 527, 528, 532, 537, 538, 540, 545, 546, 552, 554, 557, 559, 566, 573, 575, 577, 578, 581, 582, 585, 589, 590, 596, 599, 600, 602, 604, 605, 610, 611, 615, 620, 621, 625, 626, 627, 628, 630, 632, 633, 634, 635, 637, 638, 640, 641, 642, 644, 650, 654, 658, 661, 667, 668, 671, 675, 679, 684, 688, 689, 690, 692, 694, 695, 698, 699, 700, 701, 702, 706, 707, 708, 710, 713, 715, 716, 717, 719, 720, 721, 724, 726, 727, 729, 730, 735, 741, 744, 745, 747, 749, 750, 756, 765, 775, 777, 778, 781, 788, 795, 796, 798, 800, 807, 808, 810, 812, 816, 821, 824, 828, 832, 835, 836, 837, 840, 857, 858, 862, 866, 876, 881, 883, 884, 889);
