using OpenAI.Managers;
using OpenAI;
using OpenAI.Builders;
using OpenAI.Extensions;
using OpenAI.Interfaces;
using OPENAI = OpenAI.ObjectModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.ObjectModels.SharedModels;
using OpenAI.ObjectModels.RequestModels;
using System.Text.Encodings.Web;
using System.Text.Json;
using Nintai_kids.Models;
using OpenAI.ObjectModels;
using static OpenAI.ObjectModels.Models;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Reflection;

namespace Nintai_kids.Modules
{
    internal class GPTAPI
    {
        OpenAIService openAiService;
        OPENAI.Models.Model model;
        string model_string;
        public GPTAPI(OPENAI.Models.Model _model)
        {
            openAiService = new OpenAIService(new OpenAiOptions()
            {
                ApiKey = "xx-xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"
            });
            model = _model;
            model_string = GetPropertyValue(typeof(OPENAI.Models), _model.ToString())?.ToString();

        }
        static object GetPropertyValue(Type type, string propertyName)
        {
            PropertyInfo propertyInfo = type.GetProperty(propertyName);
            return propertyInfo?.GetValue(null);
        }

        public async Task<string> GetCurrentWeather(string location, string format)
        {
            return $"El clima actual en {location} es soleado y la temperatura es de 25 grados {format}.";
        }

        public async Task<string> Obtener_Respuesta_GPT(string userPrompt, string systemPrompt)
        {

            ResponseFormat response = new ResponseFormat()
            {
                Type = StaticValues.CompletionStatics.ResponseFormat.Text
            };
            var completionResult2 = await openAiService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
            {
                Messages = new List<ChatMessage>
                {
                    ChatMessage.FromSystem(systemPrompt),
                    ChatMessage.FromUser(userPrompt)
                },
                MaxTokens = 4096,
                Model = model_string,// OPENAI.Models.Gpt_3_5_Turbo,
                ResponseFormat = response
            });

            if (completionResult2.Successful)
            {

                var choice = completionResult2.Choices.First();
                Console.WriteLine($"Message:        {choice.Message.Content}");
                return (choice.Message.Content) ?? "";
            }
            return "";
        }
        public async Task<string> Generate_History_Using_Functions(string userPrompt, string systemPrompt,List<AssetClass> personajes_lista, List<AssetClass> escenarios_lista)
        {
            //var fn4x = new FunctionDefinitionBuilder("identify_number_sequence", "Get a sequence of numbers present in the user message")
            //    .AddParameter("values", PropertyDefinition.DefineArray(PropertyDefinition.DefineNumber("Sequence of numbers specified by the user")))
            //    .Build();
            var fn1 = new FunctionDefinitionBuilder("establecer_titulo", "Establecer título de la historia")
            .AddParameter("titulo", PropertyDefinition.DefineString("Titulo de la historia, e.g. Aventura en el bosque."))
            .Validate()
            .Build();
            var fn2 = new FunctionDefinitionBuilder("establecer_escena", "Crear escena como parte de la historia.")
            .AddParameter("nombre", PropertyDefinition.DefineString("Nombre de la escena, e.g. camino al bosque."))
            .AddParameter("base_visual", PropertyDefinition.DefineEnum(getListofString(escenarios_lista), "Escenario donde sucede esta escena como parte de la historia"))
            .AddParameter("orden", PropertyDefinition.DefineInteger("numero ordenado de escena empezando del 1, no pueden existir numeros de escena repetidos"))
            .Validate()
            .Build();

            var fn3 = new FunctionDefinitionBuilder("establecer_seccion", "Crear seccion como parte de la escena.")
            .AddParameter("dialogo", PropertyDefinition.DefineString("Texto o dialogo que esta diciendo un personaje o narrador, e.g. Hola amigos que es lo que realizaremos hoy?"))
            .AddParameter("autor", PropertyDefinition.DefineEnum(getListofString(personajes_lista,"narrador"), "Autor o personaje quien esta diciendo el dialogo"))
            .AddParameter("escuchante1", PropertyDefinition.DefineEnum(getListofString(personajes_lista,""), "Nombre del personaje quien esta escuchando el dialogo, puede ser una cadena vacia si no es nadie"))
            .AddParameter("escuchante2", PropertyDefinition.DefineEnum(getListofString(personajes_lista,""), "Nombre del personaje quien esta escuchando el dialogo, puede ser una cadena vacia si no es nadie"))
            .AddParameter("orden", PropertyDefinition.DefineInteger("numero ordenado de seccion dentro de la escena empezando del 1, no pueden existir numeros repetidos dentro de cada escena"))
            .Validate()
            .Build();

            var tools = new List<ToolDefinition>()
            {
                new ToolDefinition() { Function = fn3, Type="function" }
            };



            var completionResult2 = await openAiService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
            {
                Messages = new List<ChatMessage>
                {
                    ChatMessage.FromSystem(systemPrompt),
                    ChatMessage.FromUser(userPrompt)
                },
                MaxTokens = 4096,
                Tools = tools,
                Model = OPENAI.Models.Gpt_3_5_Turbo
            });

            if (completionResult2.Successful)
            {

                var choice = completionResult2.Choices.First();
                Console.WriteLine($"Message:        {choice.Message.Content}");
                //return (choice.Message.Content) ?? "";
                var toolsCalls = choice.Message.ToolCalls;
                if (toolsCalls != null)
                {
                    foreach (var entry in toolsCalls)
                    {
                        var fncall = entry.FunctionCall;


                        //var fncall = entry.FunctionCall;
                        //if (fncall != null)
                        //{
                        //    // Obtiene el nombre de la función y los argumentos
                        //    var functionName = fncall.Function;
                        //    var arguments = fncall.Arguments;

                        //    // Verifica si la función está en el mapeo
                        //    if (functionMap.TryGetValue(functionName, out var function))
                        //    {
                        //        // Ejecuta la función con los argumentos proporcionados
                        //        var result = await function(arguments);
                        //        Console.WriteLine($"Resultado de la función {functionName}: {result}");
                        //    }
                        //    else
                        //    {
                        //        Console.WriteLine($"Función desconocida: {functionName}");
                        //    }
                        //}

                        if (fncall != null)
                        {
                            foreach (var entry2 in fncall.ParseArguments())
                            {
                                Console.WriteLine($"  {entry2.Key}: {entry2.Value}");
                            }


                        }
                    }
                }
            }
            return "";
        }


        public async Task<string> Generate_History_Format(string userPrompt, string systemPrompt, List<AssetClass> personajes_lista, List<AssetClass> escenarios_lista)
        {
            ResponseFormat response = new ResponseFormat()
            {
                Type = StaticValues.CompletionStatics.ResponseFormat.Json
            };

            var completionResult2 = await openAiService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
            {
                Messages = new List<ChatMessage>
                {
                    ChatMessage.FromSystem(systemPrompt),
                    ChatMessage.FromUser(userPrompt)
                },
                MaxTokens = 4096,
                Model = OPENAI.Models.Gpt_3_5_Turbo,
                ResponseFormat = response,

            });

            if (completionResult2.Successful)
            {
                var choice = completionResult2.Choices.First();
                Console.WriteLine($"Message:        {choice.Message.Content}");
                return (choice.Message.Content) ?? "";
            }

            return "";
        }
        private List<string> getListofString(List<AssetClass> lista, string extra = "")
        {
            List<string> retorno = new List<string>();
            foreach(AssetClass item in lista)
            {
                retorno.Add(item.Name);
            }
            if (extra != "")
                retorno.Add(extra);
            return retorno;
        }
    }
}
