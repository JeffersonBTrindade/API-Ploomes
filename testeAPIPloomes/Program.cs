using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace testeAPIPloomes
{
    class Program
    {
        static void Main(string[] args)
        {
            JObject novoCliente = new JObject();
            novoCliente.Add("Name", "Jefferson Test");
            novoCliente.Add("TypeId", 2);

            JArray cliente = RequestHandler.MakePloomesRequest($"Contacts", RestSharp.Method.POST, novoCliente);

            JObject novaNegociacao = new JObject();
            novaNegociacao.Add("Title", "Nova Negociação");
            novaNegociacao.Add("ContactId", cliente[0]["Id"].ToString());

            JArray negociacao = RequestHandler.MakePloomesRequest($"Deals", RestSharp.Method.POST, novaNegociacao);

            JObject novaTarefa = new JObject();
            novaTarefa.Add("Title", "Tarefa 1");
            novaTarefa.Add("Description", "Tarefa da Nova Negocição");
            novaTarefa.Add("ContactId", cliente[0]["Id"].ToString());
            novaTarefa.Add("DealId", negociacao[0]["Id"]);

            JArray tarefa = RequestHandler.MakePloomesRequest($"Tasks", RestSharp.Method.POST, novaTarefa);

            JObject patchNegociacao = new JObject();
            patchNegociacao.Add("Amount", "15000");

            JArray negociacaoPatch = RequestHandler.MakePloomesRequest($"Deals({(int)negociacao[0]["Id"]})", RestSharp.Method.PATCH, patchNegociacao);

            JArray tarefaTerminada = RequestHandler.MakePloomesRequest($"Tasks({tarefa[0]["Id"]})/Finish", RestSharp.Method.POST);

            JArray negociacaoGanha = RequestHandler.MakePloomesRequest($"Deals({negociacao[0]["Id"]})/Win", RestSharp.Method.POST);

            JObject desHistoricoCliente = new JObject();
            desHistoricoCliente.Add("Content", "Negócio Fechado!");

            JArray historicoCliente = RequestHandler.MakePloomesRequest($"InteractionRecords?$filter=ContactId+eq+{cliente[0]["Id"]}", RestSharp.Method.POST, desHistoricoCliente);

            Console.WriteLine(negociacao.ToString());
            Console.WriteLine(tarefa.ToString() + "\n\n");
            Console.WriteLine(negociacaoPatch.ToString() + "\n\n");
            Console.WriteLine(tarefaTerminada.ToString() + "\n\n");
            Console.WriteLine(negociacaoGanha.ToString() + "\n\n");

           // JArray historico = RequestHandler.MakePloomesRequest($"InteractionRecords?$filter=ContactId+eq+{cliente[0]["Id"]}", RestSharp.Method.GET);
            Console.WriteLine(historicoCliente.ToString() + "\n\n");





        }
    }
}
