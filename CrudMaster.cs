using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using SharkBot.Models;

namespace SharkBot
{
    public static class CrudMaster
    {
        static string apiToken = "YOUR_TOKEN"; //your restdb.io token
        private static string baseUrl = "https://YOUR_URL.restdb.io"; //your restdb.io url
        
        public static T Create<T>(string collectionPath, T createData)
        {
            // Call it by passing the object you expect as T and the collection name
            // CrudMaster.Create<T>(COLLECTION_NAME, NEW_OBJECT);
            // Example:
            // var results = CrudMaster.Create<BannedDiscordUser>("banned-discord-users", objectYouCreated);
            // saves a new BannedDiscordUser
            
            var client = new RestClient(baseUrl);
            client.AddDefaultHeader("x-apikey", apiToken);
            var request = new RestRequest($"/rest/{collectionPath}", Method.POST); 
            request.AddParameter("application/json; charset=utf-8", JsonConvert.SerializeObject(createData), ParameterType.RequestBody);

            var response = client.Execute<T>(request);
            var results = JsonConvert.DeserializeObject<T>(response.Content);
            
            Console.WriteLine($"Number of items returned: {response.Content.ToString()}");

            return results;
        }
        
        public static List<T> ReadAll<T>(string collectionPath)
        {
            // Call it by passing the object you expect as T and the collection name
            // CrudMaster.ReadAll<T>(COLLECTION_NAME);
            // Example:
            // var results = CrudMaster.ReadAll<BannedDiscordUser>("banned-discord-users");
            // returns a List of BannedDiscordUser
            
            var client = new RestClient(baseUrl);
            client.AddDefaultHeader("x-apikey", apiToken);
            var request = new RestRequest($"/rest/{collectionPath}", Method.GET); 
            var response = client.Execute<List<T>>(request);
            var results = JsonConvert.DeserializeObject<List<T>>(response.Content);
            
            Console.WriteLine($"Number of items returned: {results.Count()}");

            return results;
        }
        
        public static T ReadOne<T>(string collectionPath, string itemId)
        {
            // Call it by passing the object you expect as T, the collection name, and the items's ID
            // CrudMaster.ReadOne<T>(COLLECTION_NAME, ITEM_ID);
            // Example:
            // var results = CrudMaster.ReadOne<BannedDiscordUser>("banned-discord-users", "12345);
            // returns a single BannedDiscordUser object
            
            var client = new RestClient(baseUrl);
            client.AddDefaultHeader("x-apikey", apiToken);
            var request = new RestRequest($"/rest/{collectionPath}/{itemId}", Method.GET); 
            var response = client.Execute<T>(request);
            
            var result = JsonConvert.DeserializeObject<T>(response.Content);
            
            Console.WriteLine($"Item found: {response.Content}");

            return result;
        }
        
        public static T Update<T>(string collectionPath, T updateData, string updateById)
        {
            // Call it by passing the object you expect as T, the collection name, a modified object<T>, and its ID
            // CrudMaster.Update<T>(COLLECTION_NAME, NEW_OBJECT, OBJECT_ID);
            // Example:
            // var results = CrudMaster.Update<BannedDiscordUser>("banned-discord-users", objectYouCreated, "12345");
            // updates a saved BannedDiscordUser
            
            var client = new RestClient(baseUrl);
            client.AddDefaultHeader("x-apikey", apiToken);
            var request = new RestRequest($"/rest/{collectionPath}/{updateById}", Method.PUT); 
            
            Console.WriteLine($"SENDING {JsonConvert.SerializeObject(updateData)} TO {request.Resource}" );
            
            request.AddParameter("application/json; charset=utf-8", JsonConvert.SerializeObject(updateData), ParameterType.RequestBody);

            var response = client.Execute<T>(request);
            var results = JsonConvert.DeserializeObject<T>(response.Content);
            
            Console.WriteLine($"UPDATE RESPONSE: {response.Content.ToString()}");

            return results;
        }
        
        public static void Delete<T>(string collectionPath, string deleteById)
        {
            // Call it by passing the object you expect as T, the collection name, and the id to delete
            // CrudMaster.Delete<T>(COLLECTION_NAME, DELETE_ID);
            // Example:
            // CrudMaster.Delete<BannedDiscordUser>("banned-discord-users", "12345");
            // deletes a BannedDiscordUser
            
            var client = new RestClient(baseUrl);
            client.AddDefaultHeader("x-apikey", apiToken);
            var request = new RestRequest($"/rest/{collectionPath}/{deleteById}", Method.DELETE);

            var response = client.Execute(request);

            Console.WriteLine($"DELETE RESPONSE: {response.Content}");
        }
    }
    

    /* Examples
     
    // Get all Banned Users
    List<BannedDiscordUser> allBannedUsers = CrudMaster.ReadAll<BannedDiscordUser>("banned-discord-users");

    // Get just ONE user by restdb.io record id
    BannedDiscordUser retrievedUser = CrudMaster.ReadOne<BannedDiscordUser>("banned-discord-users", "5eb46301423f275200020104");
    
    // Change that user's Reason field
    retrievedUser.ReasonForBan = "pain in the ninja";

    // Update the change on the restdb server
    BannedDiscordUser updatedUser = CrudMaster.Update("banned-discord-users", retrievedUser, retrievedUser.PrivateId);

    // Delete the user
    CrudMaster.Delete<BannedDiscordUser>("banned-discord-user", updatedUser.PrivateId);    
    
     */

    // create a class for every collection YOU make on restdb.io
    public class BannedDiscordUser
    {
        [JsonProperty("_id")]
        public string PrivateId { get; set; }
        
        [JsonProperty("banning-server")]
        public string BanningServer { get; set; }
        
        [JsonProperty("member-id")]
        public string MemberId { get; set; }
        
        [JsonProperty("reason-for-ban")]
        public string ReasonForBan { get; set; }
    }
}