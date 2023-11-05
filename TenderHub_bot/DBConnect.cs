using System.Data.SqlClient;
using System.Diagnostics;

namespace TenderHub_bot
{
	internal static class DBConnect
	{
		private static readonly string connectionString =
			"Server=danone703.mssql.somee.com;Database=danone703;User Id=Egnrhn_SQLLogin_1;Password=nf8c61k2uw;";
 		
		private static SqlConnection SqlConnection_connection = new SqlConnection(connectionString);
		private static string strSQL;
		private static SqlCommand myCommand;

		public static List<Card> GetDBTable()
		{			
			List<Card> cards = new List<Card>();
			Card card = new Card();

			
			SqlConnection_connection.Open();

			strSQL = "SELECT * FROM Zakupki";
			myCommand = new SqlCommand(strSQL, SqlConnection_connection);
			SqlDataReader dr = myCommand.ExecuteReader();

			while (dr.Read())
			{
				cards.Add(
					new Card()
					{
						ID = (int)dr[0],
						OrganizationName = (string)dr[1],
						Description = (string)dr[2],
						ProcurementType = (string)dr[3],
						PurchaseName = (string)dr[4],
						Summa = (decimal)dr[5],
						PublicationDate = dr[5].ToString(),
						Deadline = dr[7].ToString(),
					}
				);
			}

			SqlConnection_connection.Close();
			return cards;			
		}

		public static void UpdateDBTableReviews(string user, string review)
		{
			SqlConnection_connection.Open();
			strSQL = $"INSERT INTO Reviews(UserName, Review) VALUES(N'{user}', N'{review}')";
			myCommand = new SqlCommand(strSQL, SqlConnection_connection);

			Console.WriteLine($"Добавлен {myCommand.ExecuteNonQuery()} новый отзыв");
			SqlConnection_connection.Close();
		}
	}

}
