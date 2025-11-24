using Npgsql;
using NpgsqlTypes;
using APIRagSoleItem.Models;

namespace APIRagSoleItem.Repository;

public class SoldItemRepository
{
    private readonly string _connectionString;

    public SoldItemRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task InsertAsync(SoldItem item, float[] embedding)
    {
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var cmd = new NpgsqlCommand(@"
            INSERT INTO sold_items 
                (sale_date, product_id, product_name, category,
                 quantity, unit_price, customer_id, note, embedding)
            VALUES 
                (@sale_date, @product_id, @product_name, @category,
                 @quantity, @unit_price, @customer_id, @note, @embedding)
        ", conn);

        cmd.Parameters.AddWithValue("@sale_date", item.SaleDate);
        cmd.Parameters.AddWithValue("@product_id", item.ProductId);
        cmd.Parameters.AddWithValue("@product_name", item.ProductName);
        cmd.Parameters.AddWithValue("@category", (object?)item.Category ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@quantity", item.Quantity);
        cmd.Parameters.AddWithValue("@unit_price", item.UnitPrice);
        cmd.Parameters.AddWithValue("@customer_id", (object?)item.CustomerId ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@note", (object?)item.Note ?? DBNull.Value);

        // ✅ ระบุ type vector
        //cmd.Parameters.AddWithValue("@embedding", NpgsqlDbType.Vector, embedding);
        // ❌ ไม่ใช้ NpgsqlDbType.Vector
        cmd.Parameters.AddWithValue("@embedding", embedding);
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<List<SoldItemDto>> SearchAsync(float[] queryVector)
    {
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var cmd = new NpgsqlCommand(@"
        SELECT id, product_name
        FROM sold_items
        ORDER BY embedding <-> @embed::vector
        LIMIT 10
        ", conn);

        // ❌ ไม่ใช้ NpgsqlDbType.Vector
        cmd.Parameters.AddWithValue("@embed", queryVector);
        // ✅ ระบุ type vector
        //cmd.Parameters.AddWithValue("@embed", NpgsqlDbType.Vector, queryVector);

        var result = new List<SoldItemDto>();
        //Console.WriteLine($"Query vector length: {queryVector.Length}");
        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            result.Add(new SoldItemDto
            {
                Id = reader.GetInt32(0),
                Product = reader.GetString(1)
            });
        }

        return result;
    }
}