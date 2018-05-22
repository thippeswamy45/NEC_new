using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Configuration;
namespace DataAccess
{
    public class DbAccess
    {
         public  string CS = "datasource=localhost;port=3306;database=test1; username=root; password=Mind@123;SslMode=none";

        // creating the shelf and sensors,mappings

        public void AddShelf(int _rowcount, int _colcount)
        {


            // creating the shelf 

            using (MySqlConnection addconn = new MySqlConnection(CS))
            {
                MySqlCommand addshelfcmd = new MySqlCommand("AddShelfSP", addconn);
                addshelfcmd.CommandType = System.Data.CommandType.StoredProcedure;

                addshelfcmd.Parameters.AddWithValue("rowcount", _rowcount);
                addshelfcmd.Parameters.AddWithValue("colcount", _colcount);
                addconn.Open();
                addshelfcmd.ExecuteNonQuery();

                //----------------------------------------------

                // adding the shelf into rackshelfmapping  

                MySqlCommand addrackshelfmapping = new MySqlCommand("RackShelfMappingSP", addconn);
                int _rackid = 1;
                addrackshelfmapping.CommandType = System.Data.CommandType.StoredProcedure;
                addrackshelfmapping.Parameters.AddWithValue("rackid", _rackid);
                addrackshelfmapping.ExecuteNonQuery();

                //-----------------------------------------------

                // creating the sensors and mapping them to current shelf

                for (int i = 0; i < _rowcount; i++)
                {
                    for (int j = 0; j < _colcount; j++)
                    {



                        MySqlCommand addsensorcmd = new MySqlCommand("AddSensorsSP", addconn);
                        addsensorcmd.CommandType = System.Data.CommandType.StoredProcedure;

                        addsensorcmd.ExecuteNonQuery();

                        MySqlCommand ShelfSensorMappingcmd = new MySqlCommand("ShelfSensorMappingSP", addconn);
                        ShelfSensorMappingcmd.CommandType = System.Data.CommandType.StoredProcedure;

                        ShelfSensorMappingcmd.ExecuteNonQuery();

                        // Adding the sensors of created shelf into the productshelfmapping table without productid 

                        MySqlCommand ProductSensorMappingcmd = new MySqlCommand("InsertOnlySensorIdToProductSensorMappingSP", addconn);
                        ShelfSensorMappingcmd.CommandType = System.Data.CommandType.StoredProcedure;

                        ProductSensorMappingcmd.ExecuteNonQuery();

                    }
                }

                //----------------------------------------------------

                Console.WriteLine("successs1");

            }
        }

        //------------------------------------------------------------


        public void GetShelfDetails(int _shelfId, out List<int> _sensors, out List<string> _productNames, out short _rowCount, out short _columnCount)
        {

            //get row and column count for selected shelf
            short row = 0;
            short column = 0;
            List<int> sensorIds = new List<int>();
            List<string> productNames = new List<string>();

            try
            {
                using (MySqlConnection getconn = new MySqlConnection(CS))
                {

                    MySqlCommand getshelfcmd = new MySqlCommand("GetRowColumnCountSP", getconn);
                    getshelfcmd.CommandType = System.Data.CommandType.StoredProcedure;
                    getshelfcmd.Parameters.AddWithValue("selectedshelfid", _shelfId);
                    getconn.Open();
                    MySqlDataReader rowColumn = getshelfcmd.ExecuteReader();

                    if (rowColumn.HasRows)
                    {
                        while (rowColumn.Read())
                        {
                            Console.WriteLine("{0}\t{1}", rowColumn.GetInt16(0), rowColumn.GetInt16(1));
                            row = rowColumn.GetInt16(0);
                            column = rowColumn.GetInt16(1);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No items found.");

                    }
                    rowColumn.Close();
                    //-----------------------------------------------------------

                    //get sensors list of selected shelf

                    MySqlCommand getsensorsproductnamecmd = new MySqlCommand("SensorIdProductNameForGivenShelf", getconn);
                    getsensorsproductnamecmd.CommandType = System.Data.CommandType.StoredProcedure;
                    getsensorsproductnamecmd.Parameters.AddWithValue("shelfid", _shelfId);

                    MySqlDataReader sensorsproductname = getsensorsproductnamecmd.ExecuteReader();

                    while (sensorsproductname.Read())
                    {

                        if (!sensorsproductname.IsDBNull(1))
                        {
                            productNames.Add(sensorsproductname.GetString(1));

                        }
                        else
                        {
                            productNames.Add("NA");
                        }

                        sensorIds.Add(sensorsproductname.GetInt32(0));

                    }

                    foreach (var item in sensorIds)
                    {
                        Console.WriteLine(item);
                    }

                    foreach (var item in productNames)
                    {
                        Console.WriteLine(item);
                    }
                    sensorsproductname.Close();
                    //int sensorcount = 0;
                    /*  for (int i = 0; i < row; i++)
                      {
                          for (int j = 0; j < column; j++)
                          {

                              Console.Write(sensorIds[sensorcount]);
                              Console.Write("\t");
                              sensorcount++;
                          }

                          Console.Write("\n");
                      }*/
                    //---------------------------------------------------------

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            _sensors = sensorIds;
            _productNames = productNames;
            _rowCount = row;
            _columnCount = column;
            Console.WriteLine("successs2");

        }


        public void ProductSensorMapping(string _selectedProductName, int _sensorId)
        {

            using (MySqlConnection addconn = new MySqlConnection(CS))
            {

                addconn.Open();

                MySqlCommand ProductSensorMappingCmd = new MySqlCommand("ProductSensorMappingSP", addconn);
                ProductSensorMappingCmd.CommandType = System.Data.CommandType.StoredProcedure;
                ProductSensorMappingCmd.Parameters.AddWithValue("selectedproductname", _selectedProductName);
                ProductSensorMappingCmd.Parameters.AddWithValue("sensorId", _sensorId);

                ProductSensorMappingCmd.ExecuteNonQuery();

                Console.WriteLine("success3");


            }

        }


        public void GetRackShelfDetails(out List<int> _rackids, out List<int> _shelfids)
        {
            List<int> rackIds = new List<int>();
            List<int> shelfIds = new List<int>();
            using (MySqlConnection getconn = new MySqlConnection(CS))
            {
                getconn.Open();
                MySqlCommand getracklistcmd = new MySqlCommand("GetRackIdListSP", getconn);
                getracklistcmd.CommandType = System.Data.CommandType.StoredProcedure;


                MySqlDataReader RackIdList = getracklistcmd.ExecuteReader();

                if (RackIdList.HasRows)
                {

                    while (RackIdList.Read())
                    {
                
                        rackIds.Add(Convert.ToInt16(RackIdList["rackid"]));

                    }
                }
                else
                {
                    Console.WriteLine("No items found.");

                }
                RackIdList.Close();

                MySqlCommand getshelflistcmd = new MySqlCommand("GetShelfIdListSP", getconn);
                getshelflistcmd.CommandType = System.Data.CommandType.StoredProcedure;


                MySqlDataReader ShelfIdList = getshelflistcmd.ExecuteReader();

                if (ShelfIdList.HasRows)
                {

                    while (ShelfIdList.Read())
                    {
                        
                        shelfIds.Add(Convert.ToInt16(ShelfIdList["shelfid"]));

                    }
                }
                else
                {
                    Console.WriteLine("No items found.");

                }
                ShelfIdList.Close();
            }
           
          
            _rackids = rackIds;
            _shelfids = shelfIds;

        }

    }


    class Program
    {
        public static void Main()
        {
            List<int> rackIds = new List<int>();
            List<int> shelfIds = new List<int>();
            List<int> sensorIds = new List<int>();
            List<string> productNames = new List<string>();
            DbAccess db = new DbAccess();
            int row = 0;
            int col = 0;
         //   db.AddShelf(3, 3);
         //   db.GetShelfDetails(1, out sensorIds, out productNames, out row, out col);
         //   db.ProductSensorMapping("pepsi",2);

            db.GetRackShelfDetails(out rackIds, out shelfIds);

              foreach (var item in rackIds)
              {
                  Console.Write(item);
              }
              Console.WriteLine("\n");
              foreach (var item in shelfIds)
              {
                  Console.Write(item);
              }

           /* foreach (var item in sensorIds)
            {
                Console.WriteLine(item);
            }

            foreach (var item in productNames)
            {
                Console.WriteLine(item);
            }*/
            Console.Read();
        }
    }
}

