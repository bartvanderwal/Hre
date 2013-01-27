insert into my_aspnet_applications (id, name, description) values 
('1', 'HRE', 'Het Rondje Eilanden Website');

insert into my_aspnet_users(id, applicationid, name, isAnonymous, lastActivityDate) values
(2, 1, 'Admin', 0, CURRENT_TIMESTAMP);

insert into my_aspnet_roles (id, applicationid, name) values 
('1', '2', 'HRE2012Deelnemer'),
('2', '2', 'Admin'),
('3', '2', 'HRE2012Vrijwilliger'),
('4', '2', 'Geinteresseerde'),
('5', '2', 'HRE2012Sponsor');

insert into my_aspnet_usersinroles(userid, roleid) values
('1','2');