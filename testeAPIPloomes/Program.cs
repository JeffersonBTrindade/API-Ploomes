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
            novoCliente.Add("Name", "teste cliente 100");
            novoCliente.Add("TypeId", 2);

            JArray cliente = RequestHandler.MakePloomesRequest($"Contacts", RestSharp.Method.POST, novoCliente);

            JObject novaNegociacao = new JObject();
            novaNegociacao.Add("Title", "Nova Negociação 100");
            novaNegociacao.Add("ContactId", cliente[0]["Id"].ToString());

            JArray negociacao = RequestHandler.MakePloomesRequest($"Deals", RestSharp.Method.POST, novaNegociacao);

            JObject novaTarefa = new JObject();
            novaTarefa.Add("Title", "Tarefa 1000");
            novaTarefa.Add("Description", "Tarefa da Nova Negocição 100");
            novaTarefa.Add("DealId", negociacao[0]["Id"]);

            JArray tarefa = RequestHandler.MakePloomesRequest($"Tasks", RestSharp.Method.POST, novaTarefa);

            JObject patchNegociacao = new JObject();
            patchNegociacao.Add("Amount", "15000");

            JArray negociacaoPatch = RequestHandler.MakePloomesRequest($"Deals({(int)negociacao[0]["Id"]})", RestSharp.Method.PATCH, patchNegociacao);

            JArray tarefaTerminada = RequestHandler.MakePloomesRequest($"Tasks({tarefa[0]["Id"]})/Finish", RestSharp.Method.POST);

            JArray negociacaoGanha = RequestHandler.MakePloomesRequest($"Deals({negociacao[0]["Id"]})/Win", RestSharp.Method.POST);

            JObject desHistoricoCliente = new JObject();
            desHistoricoCliente.Add("ContactId", cliente[0]["Id"]);
            desHistoricoCliente.Add("Content", "Negócio Fechado!");

            JArray historicoCliente = RequestHandler.MakePloomesRequest($"InteractionRecords", RestSharp.Method.POST, desHistoricoCliente);
        }
    }
}
