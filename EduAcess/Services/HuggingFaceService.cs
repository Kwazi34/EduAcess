using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EduAcess.Services
{
	public class GroqService
	{
		private readonly string _apiKey;
		private static readonly HttpClient _client = new HttpClient();

		public GroqService(string apiKey)
		{
			_apiKey = apiKey;
		}

		public async Task<string> GetChatResponseAsync(string userMessage)
		{
			// Groq's OpenAI-compatible endpoint
			var url = "https://api.groq.com/openai/v1/chat/completions";

			// ✅ The current active free model on Groq (replaces retired llama models)
			var modelId = "openai/gpt-oss-120b";

			var payload = new
			{
				model = modelId,
				messages = new[]
				{
					new {
						role = "system",
						content = "You are the EduAccess Assistant, a helpful career guide for South African learners. You help them find universities, TVET colleges, and bursaries. Use South African context (NSFAS, APS, Matric). Keep answers concise and friendly."
					},
					new {
						role = "user",
						content = userMessage
					}
				},
				max_tokens = 800, // Increased because this is a reasoning model
				temperature = 0.7
			};

			var jsonPayload = JsonConvert.SerializeObject(payload);
			var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

			_client.DefaultRequestHeaders.Clear();
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

			try
			{
				var response = await _client.PostAsync(url, content);
				var responseString = await response.Content.ReadAsStringAsync();

				if (!response.IsSuccessStatusCode)
				{
					// Log the exact error to Visual Studio Output for debugging
					System.Diagnostics.Trace.TraceError($"Groq Status: {response.StatusCode} - Body: {responseString}");

					if (response.StatusCode == HttpStatusCode.Unauthorized)
						return "My API key is invalid. Please check your Groq key.";

					if ((int)response.StatusCode == 429)
						return "I'm receiving too many messages right now. Please wait a moment.";

					// If the model itself is unavailable (404/400)
					if ((int)response.StatusCode == 404 || (int)response.StatusCode == 400)
						return "My AI model is currently unavailable. Please try again in a few minutes.";

					return "I'm having trouble connecting to my AI brain. Please try again.";
				}

				var jsonResponse = JObject.Parse(responseString);
				var reply = jsonResponse["choices"]?[0]?["message"]?["content"]?.ToString();

				return reply?.Trim() ?? "I couldn't generate a response.";
			}
			catch (Exception ex)
			{
				System.Diagnostics.Trace.TraceError("Groq Exception: " + ex.Message);
				return "Oops! My connection dropped. Please try again.";
			}
		}
	}
}