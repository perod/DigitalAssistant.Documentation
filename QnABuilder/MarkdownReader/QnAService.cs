using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownReader
{
    public class QnAService
    {
        HttpClient _client;
        private Configuration _configuration;

        public QnAService()
        {
            _configuration = new Configuration();
            _client = new HttpClient();
        }

        public async Task<List<QuestionWithAnswer>> Get()
        {
            List<QuestionWithAnswer> dtos = new List<QuestionWithAnswer>();
            var tsvContent = await GetTsvFile();

            if(!string.IsNullOrWhiteSpace(tsvContent))
            {
                var lines = tsvContent.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                lines.RemoveAt(0);

                foreach(var line in lines)
                {
                    var sLine = line.Split(new[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
                    if(sLine.Length == 3)
                    {
                        dtos.Add(new QuestionWithAnswer
                        {
                            Question = sLine[0],
                            Answer = sLine[1]//,
                            //Source = sLine[2]
                        });
                    }
                }
            }
            return dtos;
        }


        public async Task<bool> Update(List<QuestionWithAnswer> pairsToAdd = null, List<QuestionWithAnswer> pairsToDelete = null)
        {
            var url = GetKnowledgebaseUrl();
            
            using (var requestMessage = new HttpRequestMessage(new HttpMethod("PATCH"), url))
            {
                requestMessage.Headers.Add("Ocp-Apim-Subscription-Key", _configuration.QnAKSubscriptionKey);
                requestMessage.Content = new ObjectContent<UpdateDto>(new UpdateDto(pairsToAdd, pairsToDelete), new JsonMediaTypeFormatter(), "application/json");
                var response = await _client.SendAsync(requestMessage);
                if(response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var errorMsg = response.Content != null
                       ? await response.Content.ReadAsStringAsync()
                       : "Unable to update knowledgebase";

                    throw new HttpRequestException(errorMsg);
                }
            }
        }

        public async Task<bool> Publish()
        {
            var url = GetKnowledgebaseUrl();

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Put, url))
            {
                requestMessage.Headers.Add("Ocp-Apim-Subscription-Key", _configuration.QnAKSubscriptionKey);

                var response = await _client.SendAsync(requestMessage);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var errorMsg = response.Content != null
                       ? await response.Content.ReadAsStringAsync()
                       : "Unable to update knowledgebase";

                    throw new HttpRequestException(errorMsg);
                }
            }
        }

        private async Task<string> GetTsvFile()
        {
            var uri = await GetSasUriForBlobStore();

            if(!string.IsNullOrWhiteSpace(uri))
            {
                using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri))
                {
                    var response = await _client.SendAsync(requestMessage);
                    if(response.IsSuccessStatusCode)
                    {
                        var tsvFile = await response.Content.ReadAsStringAsync();
                        return tsvFile;
                    }
                    else
                    {
                        var errorMsg = response.Content != null
                       ? await response.Content.ReadAsStringAsync()
                       : "Unable to get tsv content from SASUri provided by QnAMaker-get";

                        throw new HttpRequestException(errorMsg);
                    }
                }   
            }
            else
            {
                return "Unable to get url to tsv file";
            }
        }


        private async Task<string> GetSasUriForBlobStore()
        {
            var url = GetKnowledgebaseUrl();

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, url))
            {
                requestMessage.Headers.Add("Ocp-Apim-Subscription-Key", _configuration.QnAKSubscriptionKey);
                var response = await _client.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    var uri = await response.Content.ReadAsAsync<string>();
                    return uri;
                }
                else
                {
                    var errorMsg = response.Content != null
                        ? await response.Content.ReadAsStringAsync()
                        : "Unable to get SAS uri for tsv file in blob storage.";

                    throw new HttpRequestException(errorMsg);
                }
            }
        }

        private string GetKnowledgebaseUrl()
        {
            return $"{_configuration.QnABaseUrl.TrimEnd(new[] { '/' })}/knowledgebases/{_configuration.QnAKnowledgebaseKey}";
        }
    }
}



