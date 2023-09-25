using AtvAPICep.Models;
using Newtonsoft.Json;
using System;
using System.Net;

namespace AtvAPICep.ViaCEP
{

    public class ViaCep
    {

        private const String ViaCepUri = "https://viacep.com.br/ws/";

        public async static Task<ViaCepEndereco?> GetEndereco(String cep)
        {
            String uri = ViaCepUri + cep + "/json/";

            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync(uri);

            return JsonConvert.DeserializeObject<ViaCepEndereco>(await response.Content.ReadAsStringAsync()) ?? null;
        }

    }
}
