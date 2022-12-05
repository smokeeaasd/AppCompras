using AppCompras.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace AppCompras.Helpers
{
    public class SQLiteDBHelper
    {
        readonly SQLiteAsyncConnection sqliteConn;

        public SQLiteDBHelper(string path)
        {
            sqliteConn = new SQLiteAsyncConnection(path);

            sqliteConn.CreateTableAsync<Produto>().Wait();
        }

        public Task<int> Insert(Produto p)
        {
            return sqliteConn.InsertAsync(p);
        }

        public Task<List<Produto>> Update(Produto p)
        {
            string sql = "UPDATE Produto SET Descricao = ?, Quantidade = ?, Preco = ? WHERE id = ?";

            return sqliteConn.QueryAsync<Produto>(sql, p.Descricao, p.Quantidade, p.Preco, p.Id);
        }

        public Task<List<Produto>> GetAll()
        {
            return sqliteConn.Table<Produto>().ToListAsync();
        }

        public Task<int> Delete(int id)
        {
            return sqliteConn.Table<Produto>().DeleteAsync(i => i.Id == id);
        }

        public Task<List<Produto>> Search(string query)
        {
            string sql = $"SELECT * FROM Produto WHERE Descricao LIKE '%{query}%'";

            return sqliteConn.QueryAsync<Produto>(sql);
        }
    }
}
