using System;
using System.Threading.Tasks;
using FluentHttpRequest;
namespace Enquete.Portal.SBN
{

    public static class Program
    {
        static int TotalVotosEnviados = 0;
        struct Opcao
        {
            public static string Sim = "2";
            public static string Nao = "4";
        }
        public static Task Main(string[] args)
        {
            Console.Title = "Pesquisa: Portal SBN";
            Console.CursorVisible = false;               
            Console.WriteLine("Iniciando envio de dados");  
            do
            {
                try
                {
                    var request = RequestBuilder
                      .Create("https://www.portalsbn.com.br/enquete/votar")
                      .AddBodyParam("question_id", Opcao.Nao)
                      .AddBodyParam("channel_id", "2")
                      .Post<string>();
                    TotalVotosEnviados++;
                    Console.Clear();
                    Console.WriteLine(PlacarVotacao(request));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                   
                }
                //Descomentar linha abaixo para dar uma pausa antes de cada envio
                //System.Threading.Thread.Sleep(50);
            } while (true);
        }
        private static string PlacarVotacao(string request)
        {
            var inicio = request.IndexOf(@"<h1>N\u00c3O (") + @"<h1>N\u00c3O (".Length;
            var final = request.IndexOf(@"<", inicio) - 1;
            var placarNão = request.Substring(inicio, final - inicio);
            inicio = request.IndexOf(@"<h1>SIM (") + @"<h1>SIM (".Length;
            final = request.IndexOf(@"<", inicio) - 1;
            var placarSim = request.Substring(inicio, final - inicio);
            var placar = $"\tVocê é a favor do Impeachment de Bolsonaro?" +
                $"\n\t\tSIM: {placarSim}" +
                $"\n\t\tNÃO: {placarNão}" +
                $"\n\n\n\n\t\tVOTOS ENVIADOS: {TotalVotosEnviados}";
            return placar;
        }
    }
}