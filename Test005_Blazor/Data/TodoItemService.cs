using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Test005_Blazor.Data
{
    public class TodoItemService
    {
        public async Task<string> GetTokenAsync(string userEmail, string userPassword, string connectionAuthenticationString)
        {
            // Тело запроса
            Dictionary<string, string> userData = new Dictionary<string, string>
            {
                { "Username", userEmail },
                { "Password", userPassword },
            };

            var jsonData = JsonConvert.SerializeObject(userData, Formatting.Indented);

            // Создать Http-клиент для подключения
            HttpClient client = new HttpClient();

            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            try
            {
                // Выполнить подключение к API, используя метод POST
                HttpResponseMessage response = await client.PostAsync(connectionAuthenticationString, content);

                // Отобразить текст ответа на странице приложения
                string responseStringText = CreateAnswerResponseText(response);

                // Проверить, что получен правильный ответ от сервера
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    // Преобразовать ответ и получить из него token
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    Dictionary<string, string> theJson = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResponse);

                    string tokenString = theJson["token"];
                    return tokenString;
                }
                else
                {
                    return response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        private string CreateAnswerResponseText(HttpResponseMessage response)
        {
            string responseText = $"Status code: {response.StatusCode.ToString()} \n" +
                $"\n" +
                $"Content: {response.Content.ToString()}";

            return responseText;
        }

        public async Task<List<TodoItem>> GetTasksListUsingTokenAsync(string connectionAPIString, string access_token)
        {
            // Объявить переменную - список объектов класса TodoItem
            List<TodoItem> todoItemsCollection = new List<TodoItem>();

            // Создать Http-клиент для подключения
            HttpClient client = new HttpClient();

            //Добавить заголовок в наш GET-запрос, в котором прописать полученный ранее токен 
            var headers = client.DefaultRequestHeaders;
            headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", access_token);

            try
            {
                // Выполнить подключение к API, используя метод GET
                Uri requestUri = new Uri(connectionAPIString);
                HttpResponseMessage httpResponse = await client.GetAsync(requestUri);

                // Отобразить текст ответа на странице приложения
                string responseStringText = CreateAnswerResponseText(httpResponse);

                // Проверить, что получен правильнй ответ от сервера
                httpResponse.EnsureSuccessStatusCode();

                // Преобразовать ответ и получить из него массив json-объектов (задач)
                string httpResponseBody = await httpResponse.Content.ReadAsStringAsync();

                List<TodoItem> todoItemList = JsonConvert.DeserializeObject<List<TodoItem>>(httpResponseBody);

                // Перебрать полученный список объектов класса TodoItem
                // и поместить элементы в коллекцию (для отображения на странице нашего приложения)
                foreach (var todoItem in todoItemList)
                {
                    todoItemsCollection.Add(todoItem);
                }

                return todoItemsCollection;
            }
            catch (Exception ex)
            {
                //responseStringBox.Text = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
                return todoItemsCollection;
            }
        }

        public async Task<List<TodoItem>> AddNewTodoItemUsingTokenAsync(string connectionAPIString, string access_token, string newTodoName, string newTodoDescription)
        {
            // Объявить переменную - список объектов класса TodoItem
            List<TodoItem> todoItemsCollection = new List<TodoItem>();

            HttpClient client = new HttpClient();

            //Добавить заголовок в наш POST-запрос, в котором прописать полученный ранее токен 
            var headers = client.DefaultRequestHeaders;
            headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", access_token);

            // Тело запроса
            Dictionary<string, string> newToDoData = new Dictionary<string, string>
            {
                { "Name", newTodoName },
                { "Description", newTodoDescription },
            };

            var jsonData = JsonConvert.SerializeObject(newToDoData, Formatting.Indented);

            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            try
            {
                // Выполнить подключение к API, используя метод POST
                Uri requestUri = new Uri(connectionAPIString);
                HttpResponseMessage httpResponse = await client.PostAsync(requestUri, content);

                // Отобразить текст ответа на странице приложения
                string responseStringText = CreateAnswerResponseText(httpResponse);

                // Проверить, что получен правильнй ответ от сервера
                httpResponse.EnsureSuccessStatusCode();

                // Преобразовать ответ и получить из него json-объект (новую, только что созданную на веб-сервере задачу)
                string httpResponseBody = await httpResponse.Content.ReadAsStringAsync();

                TodoItem todoItem = JsonConvert.DeserializeObject<TodoItem>(httpResponseBody);

                todoItemsCollection.Add(todoItem);

                return todoItemsCollection;
            }
            catch (Exception ex)
            {
                //responseStringBox.Text = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
                return todoItemsCollection;
            }
        }
    }
}
