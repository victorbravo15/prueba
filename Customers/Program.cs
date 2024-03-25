using System.Data;
using System.Data.SqlClient;

class Program
{
    static void Main()
    {
        string connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=Prueba;Integrated Security=True";

        string csvFilePath = "Files\\Customers.csv";

        // Crear una conexión SQL
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            // Abrir la conexión
            connection.Open();

            // Crear un objeto SqlBulkCopy
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
            {
                // Establecer el nombre de la tabla de destino
                bulkCopy.DestinationTableName = "Customers";

                try
                {
                    // Configurar las columnas de mapeo entre el archivo CSV y la tabla de la base de datos
                    bulkCopy.ColumnMappings.Add("Id", "Id");
                    bulkCopy.ColumnMappings.Add("Name", "Name");
                    bulkCopy.ColumnMappings.Add("Address", "Address");
                    bulkCopy.ColumnMappings.Add("City", "City");
                    bulkCopy.ColumnMappings.Add("Country", "Country");
                    bulkCopy.ColumnMappings.Add("PostalCode", "PostalCode");
                    bulkCopy.ColumnMappings.Add("Phone", "Phone");

                    // Cargar los datos del archivo CSV en la tabla de la base de datos
                    using (StreamReader reader = new StreamReader(csvFilePath))
                    {
                        // Leer la primera línea para omitir los encabezados
                        reader.ReadLine();

                        // Ejecutar la copia masiva de datos
                        bulkCopy.WriteToServer(CreateDataTableFromCsv(reader));
                    }

                    Console.WriteLine("Datos cargados correctamente en la tabla Customers.");

                    // Imprimir toda la tabla Customers
                    PrintCustomersTable(connection);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
    }

    // Método para crear un DataTable a partir de un archivo CSV
    static DataTable CreateDataTableFromCsv(StreamReader reader)
    {
        DataTable dataTable = new DataTable();

        // Agregar las columnas al DataTable
        dataTable.Columns.Add("Id");
        dataTable.Columns.Add("Name");
        dataTable.Columns.Add("Address");
        dataTable.Columns.Add("City");
        dataTable.Columns.Add("Country");
        dataTable.Columns.Add("PostalCode");
        dataTable.Columns.Add("Phone");

        // Leer las líneas restantes del archivo CSV y agregarlas como filas al DataTable
        while (!reader.EndOfStream)
        {
            string[] fields = reader.ReadLine().Split(';');
            dataTable.Rows.Add(fields);
        }

        return dataTable;
    }

    // Método para imprimir toda la tabla Customers
    static void PrintCustomersTable(SqlConnection connection)
    {
        string query = "SELECT * FROM Customers";

        using (SqlCommand command = new SqlCommand(query, connection))
        {
            using (SqlDataReader reader = command.ExecuteReader())
            {
                Console.WriteLine("Tabla Customers:");
                Console.WriteLine("Id\tName\tAddress\tCity\tCountry\tPostalCode\tPhone");
                while (reader.Read())
                {
                    Console.WriteLine($"{reader["Id"]}\t{reader["Name"]}\t{reader["Address"]}\t{reader["City"]}\t{reader["Country"]}\t{reader["PostalCode"]}\t{reader["Phone"]}");
                }
            }
        }
    }
}