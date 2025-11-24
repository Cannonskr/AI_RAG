namespace APIRagSoleItem.Services;
using System.Net.Http.Json;

public class OllamaEmbeddingResponse
{
    public float[] Embedding { get; set; } = Array.Empty<float>();
}

public class EmbeddingService
{
    
        private readonly string _endpoint;
        private readonly string _model;
        private readonly HttpClient _client;

        public EmbeddingService(string endpoint, string model)  // <- ต้อง 2
        {
            _endpoint = endpoint;
            _model = model;
            _client = new HttpClient();
        }

        public async Task<float[]> GetEmbeddingAsync(string text)
        {
            var request = new { model = _model, prompt = text };
            var response = await _client.PostAsJsonAsync(_endpoint, request);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<OllamaEmbeddingResponse>();
            return result!.Embedding;
        }

        private class OllamaEmbeddingResponse
        {
            public float[] Embedding { get; set; }
        }
    
    private readonly HttpClient _http;

    public EmbeddingService(HttpClient http)
    {
        _http = http;
        _http.BaseAddress = new Uri("http://localhost:11434");
    }
    
}