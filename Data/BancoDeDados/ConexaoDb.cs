using Data.Config;
using Microsoft.Extensions.Configuration;

namespace Data.BancoDeDados
{
    public static class ConexaoBanco
    {
        public static string ObterStringConexao()
        {
            IConfigurationRoot config = ConfigHelper.LoadConfiguration();

            string caminhoArquivo = config.GetSection("DataSettings:ConexaoBanco").Value
                                    ?? throw new InvalidOperationException("ConexaoBanco não encontrada nas configurações.");

            return $"Data Source={caminhoArquivo}";
        }
    }
}