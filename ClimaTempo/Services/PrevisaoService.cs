﻿using System.Diagnostics;
using System.Text.Json;
using ClimaTempo.Models;


namespace ClimaTempo.Services
{
    public class PrevisaoService
    {
        private HttpClient client;
        private Previsao previsao;
        private Previsao previsaoProximosDias;
        private JsonSerializerOptions options;

        Uri uri = new Uri("https://brasilapi.com.br/api/cptec/v1/clima/previsao/");

        public PrevisaoService()
        {
            client = new HttpClient();
            options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        public async Task<Previsao> GetPrevisaoById(int cityCode)
        {
            Uri requestUri = new Uri($"{uri}/{cityCode}");
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    previsao = JsonSerializer.Deserialize<Previsao>(content, options);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return previsao;
        }
        public async Task<Previsao> GetPrevisaoForXDaysById(int cityCode, int days)
        {
            days = 3;
            Uri requestUri = new Uri($"{uri}/{cityCode}/ {days}");
            try
            {
                HttpResponseMessage response = await client.GetAsync(requestUri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    previsaoProximosDias = JsonSerializer.Deserialize<Previsao>(content, options);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return previsaoProximosDias;
        }

        public async Task<Previsao> GetDetalhesPrevisaoById(int cityCode)
        {
            Uri requestUri = new Uri($"{uri}/{cityCode}");
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    previsao = JsonSerializer.Deserialize<Previsao>(content, options);
                }
                else
                {
                    Previsao previsao = new Previsao();
                    Clima clima = new Clima();
                    previsao.Estado = "SP";
                    previsao.Cidade = "Sao Paulo";
                    previsao.clima.Add(clima);
                    clima.Max = 30;
                    clima.Min = 15;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return previsao;
        }
    }
}
