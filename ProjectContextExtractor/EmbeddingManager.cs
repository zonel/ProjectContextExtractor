using Pinecone;
using System.ClientModel;
using OpenAI.Embeddings;

namespace ProjectContextExtractor
{
    internal sealed class EmbeddingManager(Configuration config, ProjectContextExtractor extractor)
    {
        private readonly EmbeddingClient _openAiEmbeddingsModel = new("text-embedding-ada-002", new ApiKeyCredential(config.OpenAiApiKey));
        private readonly PineconeClient _pineconeClient = new(config.PineconeApiKey);
        private readonly string _indexName = "projectcontextextractor";

        public async Task GenerateAndStoreEmbeddingsAsync()
        {
            var projectContext = extractor.GetFullContext();

            var embedding = await GenerateEmbeddingAsync(projectContext);
            
            await StoreEmbeddingInPinecone(embedding);
        }

        private async Task<float[]> GenerateEmbeddingAsync(string projectContext)
        {
            var embeddingResponse = await _openAiEmbeddingsModel.GenerateEmbeddingAsync(projectContext)!;

            return embeddingResponse.Value!.Vector.ToArray();
        }

        private async Task StoreEmbeddingInPinecone(float[] embedding)
        {
            var vector = new Vector
            {
                Id = Guid.NewGuid().ToString(),
                Values = new ReadOnlyMemory<float>(embedding)
            };

            var upsertRequest = new UpsertRequest
            {
                Vectors = new List<Vector> { vector },
                Namespace = "Default"
            };

            var index = _pineconeClient.Index(_indexName);

            await index.UpsertAsync(upsertRequest);
        }
    }
}
