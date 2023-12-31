/* Credit is given where it is due.  With Vorspire's help, these gumps stay on the page you are on when you select an item.  For the life of me I couldn't figure it out.
Espcevan created the original AddWall and AddStair gump which helped with creating the other gumps.  AddDoor and AddSign are part of ServUO's scripts but are added into this
handy little tool.

TheArt came up with the idea to put make a tool that can help those who can't use Pandora or other third party tools due to various reasons.*/
using System;
using Server;
using Server.Commands;
using Server.Gumps;
using Server.Network;

namespace Server.Gumps
{
    public class AddRoofGump : Gump
    {
        public static void Initialize()
        {
            CommandSystem.Register(
                "AddRoof",
                AccessLevel.GameMaster,
                new CommandEventHandler(AddRoof_OnCommand)
            );
        }

        [Usage("AddRoof")]
        [Description("Displays a menu from which you can interactively add Ground Statics.")]
        public static void AddRoof_OnCommand(CommandEventArgs e)
        {
            e.Mobile.CloseGump(typeof(AddRoofGump));
            e.Mobile.SendGump(new AddRoofGump());
        }

        public static RoofInfo[] m_Types = new RoofInfo[]
        {
            #region Roofs

            new RoofInfo(1414),
            new RoofInfo(1415),
            new RoofInfo(1416),
            new RoofInfo(1417),
            new RoofInfo(1418),
            new RoofInfo(1419),
            new RoofInfo(1420),
            new RoofInfo(1421),
            new RoofInfo(1422),
            new RoofInfo(1423),
            new RoofInfo(1424),
            new RoofInfo(1427),
            new RoofInfo(1428),
            new RoofInfo(1429),
            new RoofInfo(1430),
            new RoofInfo(1431),
            new RoofInfo(1432),
            new RoofInfo(1433),
            new RoofInfo(1434),
            new RoofInfo(1435),
            new RoofInfo(1436),
            new RoofInfo(1437),
            new RoofInfo(1438),
            new RoofInfo(1439),
            new RoofInfo(1440),
            new RoofInfo(1441),
            new RoofInfo(1442),
            new RoofInfo(1443),
            new RoofInfo(1444),
            new RoofInfo(1445),
            new RoofInfo(1446),
            new RoofInfo(1447),
            new RoofInfo(1448),
            new RoofInfo(1449),
            new RoofInfo(1450),
            new RoofInfo(1451),
            new RoofInfo(1452),
            new RoofInfo(1453),
            new RoofInfo(1454),
            new RoofInfo(1455),
            new RoofInfo(1456),
            new RoofInfo(1457),
            new RoofInfo(1458),
            new RoofInfo(1459),
            new RoofInfo(1460),
            new RoofInfo(1461),
            new RoofInfo(1462),
            new RoofInfo(1463),
            new RoofInfo(1464),
            new RoofInfo(1465),
            new RoofInfo(1466),
            new RoofInfo(1467),
            new RoofInfo(1468),
            new RoofInfo(1469),
            new RoofInfo(1470),
            new RoofInfo(1471),
            new RoofInfo(1472),
            new RoofInfo(1473),
            new RoofInfo(1474),
            new RoofInfo(1475),
            new RoofInfo(1476),
            new RoofInfo(1477),
            new RoofInfo(1478),
            new RoofInfo(1479),
            new RoofInfo(1480),
            new RoofInfo(1481),
            new RoofInfo(1482),
            new RoofInfo(1483),
            new RoofInfo(1484),
            new RoofInfo(1485),
            new RoofInfo(1486),
            new RoofInfo(1487),
            new RoofInfo(1488),
            new RoofInfo(1489),
            new RoofInfo(1490),
            new RoofInfo(1491),
            new RoofInfo(1492),
            new RoofInfo(1494),
            new RoofInfo(1495),
            new RoofInfo(1496),
            new RoofInfo(1497),
            new RoofInfo(1498),
            new RoofInfo(1499),
            new RoofInfo(1500),
            new RoofInfo(1501),
            new RoofInfo(1502),
            new RoofInfo(1503),
            new RoofInfo(1504),
            new RoofInfo(1505),
            new RoofInfo(1506),
            new RoofInfo(1507),
            new RoofInfo(1508),
            new RoofInfo(1509),
            new RoofInfo(1510),
            new RoofInfo(1511),
            new RoofInfo(1512),
            new RoofInfo(1513),
            new RoofInfo(1514),
            new RoofInfo(1515),
            new RoofInfo(1516),
            new RoofInfo(1519),
            new RoofInfo(1520),
            new RoofInfo(1521),
            new RoofInfo(1522),
            new RoofInfo(1523),
            new RoofInfo(1524),
            new RoofInfo(1527),
            new RoofInfo(1528),
            new RoofInfo(1529),
            new RoofInfo(1530),
            new RoofInfo(1531),
            new RoofInfo(1532),
            new RoofInfo(1533),
            new RoofInfo(1534),
            new RoofInfo(1535),
            new RoofInfo(1536),
            new RoofInfo(1537),
            new RoofInfo(1538),
            new RoofInfo(1539),
            new RoofInfo(1540),
            new RoofInfo(1541),
            new RoofInfo(1542),
            new RoofInfo(1543),
            new RoofInfo(1544),
            new RoofInfo(1545),
            new RoofInfo(1546),
            new RoofInfo(1547),
            new RoofInfo(1548),
            new RoofInfo(1549),
            new RoofInfo(1550),
            new RoofInfo(1551),
            new RoofInfo(1552),
            new RoofInfo(1553),
            new RoofInfo(1554),
            new RoofInfo(1555),
            new RoofInfo(1556),
            new RoofInfo(1557),
            new RoofInfo(1558),
            new RoofInfo(1559),
            new RoofInfo(1560),
            new RoofInfo(1561),
            new RoofInfo(1562),
            new RoofInfo(1563),
            new RoofInfo(1564),
            new RoofInfo(1565),
            new RoofInfo(1566),
            new RoofInfo(1567),
            new RoofInfo(1568),
            new RoofInfo(1569),
            new RoofInfo(1570),
            new RoofInfo(1571),
            new RoofInfo(1572),
            new RoofInfo(1573),
            new RoofInfo(1574),
            new RoofInfo(1575),
            new RoofInfo(1576),
            new RoofInfo(1577),
            new RoofInfo(1578),
            new RoofInfo(1579),
            new RoofInfo(1580),
            new RoofInfo(1581),
            new RoofInfo(1582),
            new RoofInfo(1583),
            new RoofInfo(1584),
            new RoofInfo(1585),
            new RoofInfo(1586),
            new RoofInfo(1587),
            new RoofInfo(1588),
            new RoofInfo(1589),
            new RoofInfo(1590),
            new RoofInfo(1591),
            new RoofInfo(1592),
            new RoofInfo(1593),
            new RoofInfo(1594),
            new RoofInfo(1595),
            new RoofInfo(1596),
            new RoofInfo(1597),
            new RoofInfo(1598),
            new RoofInfo(1608),
            new RoofInfo(1609),
            new RoofInfo(1610),
            new RoofInfo(1611),
            new RoofInfo(1612),
            new RoofInfo(1613),
            new RoofInfo(1614),
            new RoofInfo(1615),
            new RoofInfo(1616),
            new RoofInfo(1617),
            new RoofInfo(1618),
            new RoofInfo(1619),
            new RoofInfo(1620),
            new RoofInfo(1621),
            new RoofInfo(1622),
            new RoofInfo(1623),
            new RoofInfo(1624),
            new RoofInfo(1627),
            new RoofInfo(1628),
            new RoofInfo(1629),
            new RoofInfo(1630),
            new RoofInfo(1631),
            new RoofInfo(1632),
            new RoofInfo(1633),
            new RoofInfo(1634),
            new RoofInfo(1635),
            new RoofInfo(1636),
            new RoofInfo(1637),
            new RoofInfo(1638),
            new RoofInfo(1639),
            new RoofInfo(1640),
            new RoofInfo(1641),
            new RoofInfo(1642),
            new RoofInfo(1643),
            new RoofInfo(1644),
            new RoofInfo(1645),
            new RoofInfo(1646),
            new RoofInfo(1647),
            new RoofInfo(1648),
            new RoofInfo(1649),
            new RoofInfo(1650),
            new RoofInfo(1651),
            new RoofInfo(6457),
            new RoofInfo(6458),
            new RoofInfo(6459),
            new RoofInfo(6460),
            new RoofInfo(6461),
            new RoofInfo(6462),
            new RoofInfo(8520),
            new RoofInfo(8521),
            new RoofInfo(8522),
            new RoofInfo(8523),
            new RoofInfo(8524),
            new RoofInfo(8525),
            new RoofInfo(8526),
            new RoofInfo(8527),
            new RoofInfo(1652),
            new RoofInfo(8560),
            new RoofInfo(8561),
            new RoofInfo(8562),
            new RoofInfo(8563),
            new RoofInfo(8564),
            new RoofInfo(8565),
            new RoofInfo(8566),
            new RoofInfo(8567),
            new RoofInfo(8568),
            new RoofInfo(8569),
            new RoofInfo(8570),
            new RoofInfo(8571),
            new RoofInfo(8572),
            new RoofInfo(8573),
            new RoofInfo(8574),
            new RoofInfo(8575),
            new RoofInfo(8576),
            new RoofInfo(8577),
            new RoofInfo(8578),
            new RoofInfo(8579),
            new RoofInfo(8580),
            new RoofInfo(8581),
            new RoofInfo(8582),
            new RoofInfo(8583),
            new RoofInfo(8681),
            new RoofInfo(8682),
            new RoofInfo(8683),
            new RoofInfo(8684),
            new RoofInfo(8685),
            new RoofInfo(8686),
            new RoofInfo(8687),
            new RoofInfo(8688),
            new RoofInfo(9150),
            new RoofInfo(9151),
            new RoofInfo(9152),
            new RoofInfo(9153),
            new RoofInfo(9154),
            new RoofInfo(9155),
            new RoofInfo(9156),
            new RoofInfo(9157),
            new RoofInfo(9158),
            new RoofInfo(9159),
            new RoofInfo(9160),
            new RoofInfo(9161),
            new RoofInfo(9162),
            new RoofInfo(9163),
            new RoofInfo(9164),
            new RoofInfo(9165),
            new RoofInfo(9166),
            new RoofInfo(9167),
            new RoofInfo(9168),
            new RoofInfo(9169),
            new RoofInfo(9170),
            new RoofInfo(9171),
            new RoofInfo(9172),
            new RoofInfo(9173),
            new RoofInfo(9174),
            new RoofInfo(9175),
            new RoofInfo(9176),
            new RoofInfo(9177),
            new RoofInfo(9178),
            new RoofInfo(9179),
            new RoofInfo(9180),
            new RoofInfo(9181),
            new RoofInfo(9182),
            new RoofInfo(9183),
            new RoofInfo(9184),
            new RoofInfo(9185),
            new RoofInfo(9186),
            new RoofInfo(9187),
            new RoofInfo(9188),
            new RoofInfo(9189),
            new RoofInfo(9190),
            new RoofInfo(9191),
            new RoofInfo(9192),
            new RoofInfo(9193),
            new RoofInfo(9194),
            new RoofInfo(9195),
            new RoofInfo(9196),
            new RoofInfo(9197),
            new RoofInfo(9198),
            new RoofInfo(9199),
            new RoofInfo(9200),
            new RoofInfo(9201),
            new RoofInfo(9202),
            new RoofInfo(9203),
            new RoofInfo(9204),
            new RoofInfo(9205),
            new RoofInfo(9206),
            new RoofInfo(9207),
            new RoofInfo(9208),
            new RoofInfo(9209),
            new RoofInfo(9210),
            new RoofInfo(9211),
            new RoofInfo(9212),
            new RoofInfo(9213),
            new RoofInfo(9214),
            new RoofInfo(9215),
            new RoofInfo(9946),
            new RoofInfo(9947),
            new RoofInfo(9948),
            new RoofInfo(9949),
            new RoofInfo(9950),
            new RoofInfo(9951),
            new RoofInfo(9952),
            new RoofInfo(9953),
            new RoofInfo(9954),
            new RoofInfo(9955),
            new RoofInfo(9956),
            new RoofInfo(9957),
            new RoofInfo(9958),
            new RoofInfo(9959),
            new RoofInfo(9960),
            new RoofInfo(9961),
            new RoofInfo(9962),
            new RoofInfo(9963),
            new RoofInfo(9964),
            new RoofInfo(9976),
            new RoofInfo(9977),
            new RoofInfo(9978),
            new RoofInfo(9979),
            new RoofInfo(9980),
            new RoofInfo(9981),
            new RoofInfo(9982),
            new RoofInfo(9983),
            new RoofInfo(9984),
            new RoofInfo(9985),
            new RoofInfo(9986),
            new RoofInfo(9987),
            new RoofInfo(9988),
            new RoofInfo(9989),
            new RoofInfo(9990),
            new RoofInfo(9991),
            new RoofInfo(9992),
            new RoofInfo(9993),
            new RoofInfo(9994),
            new RoofInfo(9995),
            new RoofInfo(9996),
            new RoofInfo(10416),
            new RoofInfo(10417),
            new RoofInfo(10418),
            new RoofInfo(10419),
            new RoofInfo(10420),
            new RoofInfo(10421),
            new RoofInfo(10422),
            new RoofInfo(10423),
            new RoofInfo(10424),
            new RoofInfo(10425),
            new RoofInfo(10426),
            new RoofInfo(10427),
            new RoofInfo(10428),
            new RoofInfo(10429),
            new RoofInfo(10430),
            new RoofInfo(10431),
            new RoofInfo(10432),
            new RoofInfo(10433),
            new RoofInfo(10434),
            new RoofInfo(10435),
            new RoofInfo(10436),
            new RoofInfo(10437),
            new RoofInfo(10438),
            new RoofInfo(10439),
            new RoofInfo(10440),
            new RoofInfo(10441),
            new RoofInfo(10442),
            new RoofInfo(10443),
            new RoofInfo(10444),
            new RoofInfo(10445),
            new RoofInfo(10446),
            new RoofInfo(10447),
            new RoofInfo(10448),
            new RoofInfo(10449),
            new RoofInfo(10450),
            new RoofInfo(10451),
            new RoofInfo(10452),
            new RoofInfo(10453),
            new RoofInfo(10454),
            new RoofInfo(10455),
            new RoofInfo(10456),
            new RoofInfo(10457),
            new RoofInfo(10458),
            new RoofInfo(10459),
            new RoofInfo(10468),
            new RoofInfo(10469),
            new RoofInfo(10470),
            new RoofInfo(10471),
            new RoofInfo(10472),
            new RoofInfo(10473),
            new RoofInfo(10474),
            new RoofInfo(10475),
            new RoofInfo(10476),
            new RoofInfo(10477),
            new RoofInfo(10478),
            new RoofInfo(10480),
            new RoofInfo(10481),
            new RoofInfo(10482),
            new RoofInfo(10483),
            new RoofInfo(10484),
            new RoofInfo(10485),
            new RoofInfo(10486),
            new RoofInfo(10487),
            new RoofInfo(10488),
            new RoofInfo(10489),
            new RoofInfo(10490),
            new RoofInfo(10491),
            new RoofInfo(10492),
            new RoofInfo(10493),
            new RoofInfo(10494),
            new RoofInfo(10495),
            new RoofInfo(10496),
            new RoofInfo(10497),
            new RoofInfo(10498),
            new RoofInfo(10499),
            new RoofInfo(10500),
            new RoofInfo(10501),
            new RoofInfo(10502),
            new RoofInfo(10503),
            new RoofInfo(10504),
            new RoofInfo(10505),
            new RoofInfo(10506),
            new RoofInfo(10507),
            new RoofInfo(10508),
            new RoofInfo(10509),
            new RoofInfo(10510),
            new RoofInfo(10511),
            new RoofInfo(10512),
            new RoofInfo(10513),
            new RoofInfo(10514),
            new RoofInfo(10515),
            new RoofInfo(10516),
            new RoofInfo(10517),
            new RoofInfo(10518),
            new RoofInfo(10519),
            new RoofInfo(10520),
            new RoofInfo(10521),
            new RoofInfo(10522),
            new RoofInfo(10523),
            new RoofInfo(10528),
            new RoofInfo(10529),
            new RoofInfo(10530),
            new RoofInfo(10531),
            new RoofInfo(10532),
            new RoofInfo(10533),
            new RoofInfo(10534),
            new RoofInfo(10535),
            new RoofInfo(10536),
            new RoofInfo(10537),
            new RoofInfo(10538),
            new RoofInfo(10539),
            new RoofInfo(10540),
            new RoofInfo(10541),
            new RoofInfo(10542),
            new RoofInfo(10543),
            new RoofInfo(10544),
            new RoofInfo(10545),
            new RoofInfo(10546),
            new RoofInfo(10547),
            new RoofInfo(10548),
            new RoofInfo(10795),
            new RoofInfo(10796),
            new RoofInfo(10797),
            new RoofInfo(10798),
            new RoofInfo(10549),
            new RoofInfo(11137),
            new RoofInfo(11138),
            new RoofInfo(11139),
            new RoofInfo(11140),
            new RoofInfo(11141),
            new RoofInfo(11142),
            new RoofInfo(11143),
            new RoofInfo(11144),
            new RoofInfo(11145),
            new RoofInfo(11146),
            new RoofInfo(11147),
            new RoofInfo(11148),
            new RoofInfo(11149),
            new RoofInfo(11150),
            new RoofInfo(11151),
            new RoofInfo(11152),
            new RoofInfo(11153),
            new RoofInfo(11154),
            new RoofInfo(11155),
            new RoofInfo(11156),
            new RoofInfo(11157),
            new RoofInfo(11158),
            new RoofInfo(11159),
            new RoofInfo(11160),
            new RoofInfo(11161),
            new RoofInfo(11162),
            new RoofInfo(11163),
            new RoofInfo(11164),
            new RoofInfo(11165),
            new RoofInfo(11166),
            new RoofInfo(11167),
            new RoofInfo(11168),
            new RoofInfo(11169),
            new RoofInfo(11170),
            new RoofInfo(11171),
            new RoofInfo(11172),
            new RoofInfo(11173),
            new RoofInfo(11174),
            new RoofInfo(11300),
            new RoofInfo(11301),
            new RoofInfo(11302),
            new RoofInfo(11303),
            new RoofInfo(11304),
            new RoofInfo(11305),
            new RoofInfo(11306),
            new RoofInfo(11307),
            new RoofInfo(11308),
            new RoofInfo(11309),
            new RoofInfo(11310),
            new RoofInfo(11311),
            new RoofInfo(11312),
            new RoofInfo(11313),
            new RoofInfo(11314),
            new RoofInfo(11315),
            new RoofInfo(11316),
            new RoofInfo(11317),
            new RoofInfo(11318),
            new RoofInfo(11319),
            new RoofInfo(11320),
            new RoofInfo(11321),
            new RoofInfo(11322),
            new RoofInfo(11323),
            new RoofInfo(11324),
            new RoofInfo(11325),
            new RoofInfo(11327),
            new RoofInfo(11327),
            new RoofInfo(11328),
            new RoofInfo(11329),
            new RoofInfo(11330),
            new RoofInfo(11331),
            new RoofInfo(11332),
            new RoofInfo(11333),
            new RoofInfo(11334),
            new RoofInfo(11335),
            new RoofInfo(11336),
            new RoofInfo(11337),
            new RoofInfo(11338),
            new RoofInfo(11339),
            new RoofInfo(11340),
            new RoofInfo(11341),
            new RoofInfo(11342),
            new RoofInfo(11343),
            new RoofInfo(11344),
            new RoofInfo(11345),
            new RoofInfo(11346),
            new RoofInfo(11347),
            new RoofInfo(11348),
            new RoofInfo(11349),
            new RoofInfo(11350),
            new RoofInfo(11351),
            new RoofInfo(11352),
            new RoofInfo(11353),
            new RoofInfo(11354),
            new RoofInfo(11355),
            new RoofInfo(11356),
            new RoofInfo(11357),
            new RoofInfo(11358),
            new RoofInfo(11359),
            new RoofInfo(11360),
            new RoofInfo(11361),
            new RoofInfo(11362),
            new RoofInfo(11363),
            new RoofInfo(11364),
            new RoofInfo(11365),
            new RoofInfo(11366),
            new RoofInfo(11367),
            new RoofInfo(11368),
            new RoofInfo(11369),
            new RoofInfo(11370),
            new RoofInfo(11371),
            new RoofInfo(11372),
            new RoofInfo(11373),
            new RoofInfo(11374),
            new RoofInfo(11375),
            new RoofInfo(11376),
            new RoofInfo(11377),
            new RoofInfo(11378),
            new RoofInfo(11379),
            new RoofInfo(11380),
            new RoofInfo(11381),
            new RoofInfo(11382),
            new RoofInfo(11383),
            new RoofInfo(11384),
            new RoofInfo(11385),
            new RoofInfo(11386),
            new RoofInfo(11387),
            new RoofInfo(11388),
            new RoofInfo(11389),
            new RoofInfo(12063),
            new RoofInfo(12064),
            new RoofInfo(12065),
            new RoofInfo(12066),
            new RoofInfo(12067),
            new RoofInfo(12068),
            new RoofInfo(12069),
            new RoofInfo(12070),
            new RoofInfo(12071),
            new RoofInfo(12072),
            new RoofInfo(13758),
            new RoofInfo(13759),
            new RoofInfo(13760),
            new RoofInfo(13761),
            new RoofInfo(13762),
            new RoofInfo(13763),
            new RoofInfo(13764),
            new RoofInfo(13765),
            new RoofInfo(13766),
            new RoofInfo(13767),
            new RoofInfo(13768),
            new RoofInfo(13769),
            new RoofInfo(13770),
            new RoofInfo(13771),
            new RoofInfo(13772),
            new RoofInfo(13773),
            new RoofInfo(13774),
            new RoofInfo(13775),
            new RoofInfo(13776),
            new RoofInfo(13777),
            new RoofInfo(13859),
            new RoofInfo(13860),
            new RoofInfo(13861),
            new RoofInfo(13862),
            new RoofInfo(13863),
            new RoofInfo(13864),
            new RoofInfo(13865),
            new RoofInfo(13866),
            new RoofInfo(13867),
            new RoofInfo(13868),
            new RoofInfo(13869),
            new RoofInfo(13870),
            new RoofInfo(13871),
            new RoofInfo(13872),
            new RoofInfo(13873),
            new RoofInfo(13874),
            new RoofInfo(13875),
            new RoofInfo(13876),
            new RoofInfo(13877),
            new RoofInfo(19960),
            new RoofInfo(19961),
            new RoofInfo(19962),
            new RoofInfo(19963),
            new RoofInfo(19964),
            new RoofInfo(19965),
            new RoofInfo(19966),
            new RoofInfo(19967),
            new RoofInfo(19968),
            new RoofInfo(19969),
            new RoofInfo(19970),
            new RoofInfo(19971),
            new RoofInfo(19972),
            new RoofInfo(19973),
            new RoofInfo(19974),
            new RoofInfo(19975),
            new RoofInfo(19976),
            new RoofInfo(19977),
            new RoofInfo(19978),
            new RoofInfo(19979),
            new RoofInfo(19980),
            new RoofInfo(19981),
            new RoofInfo(19982),
            new RoofInfo(19983),
            new RoofInfo(19984),
            new RoofInfo(19985),
            new RoofInfo(19986),
            new RoofInfo(19987),
            new RoofInfo(19988),
            new RoofInfo(19989),
            new RoofInfo(19990),
            new RoofInfo(19991),
            new RoofInfo(19992),
            new RoofInfo(19993),
            new RoofInfo(19994),
            new RoofInfo(19995),
            new RoofInfo(19996),
            new RoofInfo(19997),
            new RoofInfo(19998),
            new RoofInfo(19999),
            new RoofInfo(20000),
            new RoofInfo(20001),
            new RoofInfo(20002),
            new RoofInfo(20003),
            new RoofInfo(20004),
            new RoofInfo(20005),
            new RoofInfo(20006),
            new RoofInfo(20007),
            new RoofInfo(20008),
            new RoofInfo(20009),
            new RoofInfo(20010),
            new RoofInfo(20011),
            new RoofInfo(20012),
            new RoofInfo(20013),
            new RoofInfo(20014),
            new RoofInfo(20015),
            new RoofInfo(20016),
            new RoofInfo(20017),
            new RoofInfo(20018),
            new RoofInfo(20019),
            new RoofInfo(20020),
            new RoofInfo(20021),
            new RoofInfo(20022),
            new RoofInfo(20023),
            new RoofInfo(20024),
            new RoofInfo(20025),
            new RoofInfo(20026),
            new RoofInfo(20027),
            new RoofInfo(20028),
            new RoofInfo(20029),
            new RoofInfo(20030),
            new RoofInfo(20031),
            new RoofInfo(20032),
            new RoofInfo(20033),
            new RoofInfo(20034),
            new RoofInfo(20035),
            new RoofInfo(20036),
            new RoofInfo(20037),
            new RoofInfo(20038),
            new RoofInfo(20039),
            new RoofInfo(20040),
            new RoofInfo(20041),
            new RoofInfo(20042),
            new RoofInfo(20043),
            new RoofInfo(20044),
            new RoofInfo(20045),
            new RoofInfo(20046),
            new RoofInfo(20047),
            new RoofInfo(20048),
            new RoofInfo(20049),
            new RoofInfo(20050),
            new RoofInfo(20051),
            new RoofInfo(20052),
            new RoofInfo(20053),
            new RoofInfo(20054),
            new RoofInfo(20055),
            new RoofInfo(20056),
            new RoofInfo(20057),
            new RoofInfo(20058),
            new RoofInfo(20059),
            new RoofInfo(20060),
            new RoofInfo(20061),
            new RoofInfo(20062),
            new RoofInfo(20063),
            new RoofInfo(20064),
            new RoofInfo(20065),
            new RoofInfo(20066),
            new RoofInfo(20067),
            new RoofInfo(20068),
            new RoofInfo(20069),
            new RoofInfo(20070),
            new RoofInfo(20071),
            new RoofInfo(20072),
            new RoofInfo(20073),
            new RoofInfo(20074),
            new RoofInfo(20075),
            new RoofInfo(20076),
            new RoofInfo(20077),
            new RoofInfo(20078),
            new RoofInfo(20079),
            new RoofInfo(20080),
            new RoofInfo(20081),
            new RoofInfo(20082),
            new RoofInfo(20083),
            new RoofInfo(20084),
            new RoofInfo(20085),
            new RoofInfo(20086),
            new RoofInfo(20087),
            new RoofInfo(20088),
            new RoofInfo(20089),
            new RoofInfo(20090),
            new RoofInfo(20091),
            new RoofInfo(20092),
            new RoofInfo(20093),
            new RoofInfo(20094),
            new RoofInfo(20095),
            new RoofInfo(20096),
            new RoofInfo(20097),
            new RoofInfo(20098),
            new RoofInfo(20099),
            new RoofInfo(20100),
            new RoofInfo(20101),
            new RoofInfo(20102),
            new RoofInfo(20103),
            new RoofInfo(20104),
            new RoofInfo(20105),
            new RoofInfo(20106),
            new RoofInfo(20107),
            new RoofInfo(20108),
            new RoofInfo(20109),
            new RoofInfo(20110),
            new RoofInfo(20111),
            new RoofInfo(20112),
            new RoofInfo(20113),
            new RoofInfo(20114),
            new RoofInfo(20115),
            new RoofInfo(20116),
            new RoofInfo(20117),
            new RoofInfo(20118),
            new RoofInfo(20119),
            new RoofInfo(20120),
            new RoofInfo(20121),
            new RoofInfo(20122),
            new RoofInfo(20123),
            new RoofInfo(20124),
            new RoofInfo(20125),
            new RoofInfo(20126),
            new RoofInfo(20127),
            new RoofInfo(20128),
            new RoofInfo(20129),
            new RoofInfo(20130),
            new RoofInfo(20131),
            new RoofInfo(20132),
            new RoofInfo(20133),
            new RoofInfo(20134),
            new RoofInfo(20135),
            new RoofInfo(20136),
            new RoofInfo(20137),
            new RoofInfo(20138),
            new RoofInfo(20139),
            new RoofInfo(20140),
            new RoofInfo(20141),
            new RoofInfo(20142),
            new RoofInfo(20143),
            new RoofInfo(20144),
            new RoofInfo(20145),
            new RoofInfo(20146),
            new RoofInfo(20147),
            new RoofInfo(20148),
            new RoofInfo(20149),
            new RoofInfo(20150),
            new RoofInfo(20151),
            new RoofInfo(20152),
            new RoofInfo(20153),
            new RoofInfo(20154),
            new RoofInfo(20155),
            new RoofInfo(20156),
            new RoofInfo(20157),
            new RoofInfo(20158),
            new RoofInfo(20159),
            new RoofInfo(20160),
            new RoofInfo(20161),
            new RoofInfo(20162),
            new RoofInfo(20163),
            new RoofInfo(20164),
            new RoofInfo(20165),
            new RoofInfo(20166),
            new RoofInfo(20167),
            new RoofInfo(20168),
            new RoofInfo(20169),
            new RoofInfo(20170),
            new RoofInfo(20171),
            new RoofInfo(20172),
            new RoofInfo(20173),
            new RoofInfo(20174),
            new RoofInfo(20175),
            new RoofInfo(20176),
            new RoofInfo(20177),
            new RoofInfo(20178),
            new RoofInfo(20179),
            new RoofInfo(20180),
            new RoofInfo(20181),
            new RoofInfo(20182),
            new RoofInfo(20183),
            new RoofInfo(20184),
            new RoofInfo(20185),
            new RoofInfo(20186),
            new RoofInfo(20187),
            new RoofInfo(20188),
            new RoofInfo(20189),
            new RoofInfo(20190),
            new RoofInfo(20191),
            new RoofInfo(20192),
            new RoofInfo(20193),
            new RoofInfo(20194),
            new RoofInfo(20195),
            new RoofInfo(20196),
            new RoofInfo(20197),
            new RoofInfo(20198),
            new RoofInfo(20199),
            new RoofInfo(20200),
            new RoofInfo(20201),
            new RoofInfo(20202),
            new RoofInfo(20203),
            new RoofInfo(20204),
            new RoofInfo(20205),
            new RoofInfo(20206),
            new RoofInfo(20207),
            new RoofInfo(20208),
            new RoofInfo(20209),
            new RoofInfo(20210),
            new RoofInfo(20211),
            new RoofInfo(20212),
            new RoofInfo(20213),
            new RoofInfo(20214),
            new RoofInfo(20215),
            new RoofInfo(20216),
            new RoofInfo(20217),
            new RoofInfo(20218),
            new RoofInfo(20219),
            new RoofInfo(20220),
            new RoofInfo(20221),
            new RoofInfo(20222),
            new RoofInfo(20223),
            new RoofInfo(20224),
            new RoofInfo(20225),
            new RoofInfo(20226),
            new RoofInfo(20227),
            new RoofInfo(20228),
            new RoofInfo(20229),
            new RoofInfo(20230),
            new RoofInfo(20231),
            new RoofInfo(20232),
            new RoofInfo(20233),
            new RoofInfo(20234),
            new RoofInfo(20235),
            new RoofInfo(20236),
            new RoofInfo(20237),
            new RoofInfo(20238),
            new RoofInfo(20239),
            new RoofInfo(20240),
            new RoofInfo(20241),
            new RoofInfo(20242),
            new RoofInfo(20243),
            new RoofInfo(20244),
            new RoofInfo(20245),
            new RoofInfo(20246),
            new RoofInfo(20247),
            new RoofInfo(20248),
            new RoofInfo(20249),
            new RoofInfo(20250),
            new RoofInfo(20251),
            new RoofInfo(20252),
            new RoofInfo(20253),
            new RoofInfo(20254),
            new RoofInfo(20255),
            new RoofInfo(20256),
            new RoofInfo(20257),
            new RoofInfo(20258),
            new RoofInfo(20259),
            new RoofInfo(20260),
            new RoofInfo(20261),
            new RoofInfo(20262),
            new RoofInfo(20263),
            new RoofInfo(20264),
            new RoofInfo(20265),
            new RoofInfo(20266),
            new RoofInfo(20267),
            new RoofInfo(20268),
            new RoofInfo(20269),
            new RoofInfo(20270),
            new RoofInfo(20271),
            new RoofInfo(20272),
            new RoofInfo(20273),
            new RoofInfo(20274),
            new RoofInfo(20275),
            new RoofInfo(20276),
            new RoofInfo(20277),
            new RoofInfo(20278),
            new RoofInfo(20279),
            new RoofInfo(20280),
            new RoofInfo(20281),
            new RoofInfo(20282),
            new RoofInfo(20283),
            new RoofInfo(20284),
            new RoofInfo(20285),
            new RoofInfo(20286),
            new RoofInfo(20287),
            new RoofInfo(20288),
            new RoofInfo(20289),
            new RoofInfo(20290),
            new RoofInfo(20291),
            new RoofInfo(20292),
            new RoofInfo(20293),
            new RoofInfo(20294),
            new RoofInfo(20295),
            new RoofInfo(20296),
            new RoofInfo(20297),
            new RoofInfo(20298),
            new RoofInfo(20299),
            new RoofInfo(20300),
            new RoofInfo(20301),
            new RoofInfo(20302),
            new RoofInfo(20303),
            new RoofInfo(20304),
            new RoofInfo(20305),
            new RoofInfo(20306),
            new RoofInfo(20307),
            new RoofInfo(20308),
            new RoofInfo(20309),
            new RoofInfo(20310),
            new RoofInfo(20311)

            #endregion
        };

        private int m_Page;

        private readonly int m_Type;

        public AddRoofGump()
            : this(0) { }

        public AddRoofGump(int page)
            : base(0, 0)
        {
            int pageCount = 1 + (m_Types.Length / 10);

            if (page >= pageCount)
                page = pageCount - 1;
            else if (page < 0)
                page = 0;

            m_Page = page;

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);

            AddBackground(0, 0, 600, 180, 3500);

            AddHtmlLocalized(15, 15, 60, 20, 1042971, page.ToString(), 0x1, false, false); // #

            AddHtmlLocalized(20, 38, 60, 20, 1043353, 0x1, false, false); // Next

            if (page + 1 < pageCount)
                AddButton(15, 55, 0xFA5, 0xFA7, 10000 + (page + 1), GumpButtonType.Reply, 0);
            else
                AddButton(15, 55, 0xFA5, 0xFA7, 10000, GumpButtonType.Reply, 0);

            AddHtmlLocalized(20, 93, 60, 20, 1011393, 0x1, false, false); // Back

            if (page > 0)
                AddButton(15, 110, 0xFAE, 0xFB0, 10000 + (page - 1), GumpButtonType.Reply, 0);
            else
                AddButton(15, 110, 0xFAE, 0xFB0, 10000, GumpButtonType.Reply, 0);

            for (int i = 0; i < 10; ++i)
            {
                int index = (page * 10) + i;
                if (index >= m_Types.Length)
                    break;

                int button = 1000000 + index;
                int offset = (i + 1) * 50;

                if (m_Types[index].m_BaseID > 0)
                {
                    AddButton(
                        25 + offset,
                        20,
                        0x2624,
                        0x2625,
                        button,
                        GumpButtonType.Reply,
                        m_Types[index].m_BaseID
                    );
                    AddItem(15 + offset, 50, m_Types[index].m_BaseID);
                }
                else
                {
                    AddImage(25 + offset, 20, 0x2625, 900);
                }
            }
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            int button = info.ButtonID;

            if (button <= 0)
                return;

            int page = m_Page;

            if (button >= 1000000)
            {
                button -= 1000000;

                CommandSystem.Handle(
                    from,
                    String.Format(
                        "{0}Tile Static {1}",
                        CommandSystem.Prefix,
                        m_Types[button].m_BaseID
                    )
                );
            }
            else if (button >= 10000)
            {
                button -= 10000;

                page = button;
            }

            from.SendGump(new AddRoofGump(page));
        }

        public class RoofInfo
        {
            public int m_BaseID;

            public RoofInfo(int baseID)
            {
                m_BaseID = baseID;
            }
        }
    }
}
