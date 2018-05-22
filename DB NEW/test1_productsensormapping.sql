/*
SQLyog Community v13.0.0 (64 bit)
MySQL - 5.6.25-log : Database - test1
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`test1` /*!40100 DEFAULT CHARACTER SET utf8 */;

USE `test1`;

/*Table structure for table `productsensormapping` */

DROP TABLE IF EXISTS `productsensormapping`;

CREATE TABLE `productsensormapping` (
  `sensorid` int(11) NOT NULL,
  `productid` int(11) DEFAULT NULL,
  PRIMARY KEY (`sensorid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/* Procedure structure for procedure `AddSensorsSP` */

/*!50003 DROP PROCEDURE IF EXISTS  `AddSensorsSP` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `AddSensorsSP`()
BEGIN
		DECLARE lastsensorid INT;
		DECLARE sensorname varchar(50);
		set lastsensorid = (select max(sensorid)+1 from sensor);
		set sensorname = concat('sensor',lastsensorid);
		INSERT INTO sensor (sensorname) VALUES (sensorname);
	END */$$
DELIMITER ;

/* Procedure structure for procedure `AddShelfSP` */

/*!50003 DROP PROCEDURE IF EXISTS  `AddShelfSP` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `AddShelfSP`(rowcount int ,colcount int)
BEGIN
		declare shelfname VARCHAR(50);
		DECLARE lastshelfid INT;
		
		SET lastshelfid = (SELECT MAX(shelfid)+1 FROM shelf);
		SET shelfname = CONCAT('shelf',lastshelfid);
		insert into shelf (shelfname,rowcount,columncount) values (shelfname,rowcount,colcount); 
	END */$$
DELIMITER ;

/* Procedure structure for procedure `GetRackShelfListSP` */

/*!50003 DROP PROCEDURE IF EXISTS  `GetRackShelfListSP` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `GetRackShelfListSP`()
BEGIN
		SELECT * FROM rackshelfmapping;
	END */$$
DELIMITER ;

/* Procedure structure for procedure `GetRowColumnCountSP` */

/*!50003 DROP PROCEDURE IF EXISTS  `GetRowColumnCountSP` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `GetRowColumnCountSP`(selectedshelfid INT)
BEGIN
		SELECT rowcount,columncount FROM shelf WHERE shelfid=selectedshelfid;
	END */$$
DELIMITER ;

/* Procedure structure for procedure `GetSensorsSP` */

/*!50003 DROP PROCEDURE IF EXISTS  `GetSensorsSP` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `GetSensorsSP`(selectedshelfid int)
BEGIN
		select sensorid from shelfsensormapping where shelfid = selectedshelfid;
	END */$$
DELIMITER ;

/* Procedure structure for procedure `InsertOnlySensorIdToProductSensorMappingSP` */

/*!50003 DROP PROCEDURE IF EXISTS  `InsertOnlySensorIdToProductSensorMappingSP` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `InsertOnlySensorIdToProductSensorMappingSP`()
BEGIN
		DECLARE PresentSensorid INT;
		SET PresentSensorid = (SELECT COUNT(sensorid) FROM sensor);
		INSERT INTO productsensormapping (sensorid) VALUES (PresentSensorid);
	END */$$
DELIMITER ;

/* Procedure structure for procedure `ProductSensorMappingSP` */

/*!50003 DROP PROCEDURE IF EXISTS  `ProductSensorMappingSP` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `ProductSensorMappingSP`(selectedproductname varchar(50),sensorId int)
BEGIN
		declare PresentProductid INT;
		
		SET PresentProductid = (select productid from product where productname=selectedproductname);
		
		
		insert INTO productsensormapping (sensorid,productid) VALUES (sensorId,PresentProductid) on duplicate key update productid= PresentProductid;
		
	END */$$
DELIMITER ;

/* Procedure structure for procedure `RackShelfMappingSP` */

/*!50003 DROP PROCEDURE IF EXISTS  `RackShelfMappingSP` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `RackShelfMappingSP`(rackid int)
BEGIN
		declare PresentShelfid int;
		set PresentShelfid = (SELECT COUNT(shelfid) FROM shelf);
		INSERT INTO rackshelfmapping (rackid,shelfid) VALUES (rackid,PresentShelfid);
	END */$$
DELIMITER ;

/* Procedure structure for procedure `SensorIdProductNameForGivenShelf` */

/*!50003 DROP PROCEDURE IF EXISTS  `SensorIdProductNameForGivenShelf` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `SensorIdProductNameForGivenShelf`(shelfid INT)
BEGIN
		SELECT ss.sensorid, p.productname
		FROM shelfsensormapping ss
		INNER JOIN productsensormapping ps ON ss.sensorid = ps.sensorid
		LEFT JOIN product p ON ps.productid = p.productid WHERE ss.shelfid=shelfid;
	END */$$
DELIMITER ;

/* Procedure structure for procedure `ShelfSensorMappingSP` */

/*!50003 DROP PROCEDURE IF EXISTS  `ShelfSensorMappingSP` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `ShelfSensorMappingSP`()
BEGIN
		DECLARE PresentShelfid INT;
		DECLARE PresentSensorid INT;
		SET PresentShelfid = (SELECT COUNT(shelfid) FROM shelf);
		
		SET PresentSensorid = (SELECT COUNT(sensorid) FROM sensor);
		INSERT INTO shelfsensormapping (shelfid,sensorid) VALUES (PresentShelfid,PresentSensorid);
	END */$$
DELIMITER ;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
