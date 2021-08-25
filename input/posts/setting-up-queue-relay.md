Title: Using Queues on Azure for message relay
Lead: Setting up queues and a messaging relay is a bit more complimicated
Published: 8/21/2021
Tags: [azure]
---

#### Overview {.bg-dark .text-light .p-2}

1. Create a service bus queue
2. Create a logic app to respond to queue messages
3. Create the workflow to pop the message off queue and send an email (parsing json along the way)
4. Post a message to queue via .Net

#### Create a service bus name space {.bg-dark .text-light .p-2}

1. In Azure Portal, select All resources and then create a service bus namespace (or use an existing one, if you have it already)
5. Select an Azure subscription
5. Select or create an Azure resource group
6. Pick a unique name
7. Pick a region for functions api (close to home)
9. Pick a pricing plan (Basic is good if all you need is queue, Standard is good if you need subscriptions, Premium is too spendy)
14. Review + Create

#### Create a service bus queue {.bg-dark .text-light .p-2}

1. In Azure Portal, select All resources and then select the namespace already created
5. Select an Queues from the context menu
5. Click + Queue
6. Pick a unique name
7. Customize settings (defaults are good)
9. Review + Create

#### Create a logic app {.bg-dark .text-light .p-2}

1. In Azure Portal, select All resources and then create a logic app
2. Only one workflow per logic app apparently
5. Select an Azure subscription
5. Select or create an Azure resource group
6. Pick a type (consumption is good unless you want more control, are using multiple subscriptions or want your own docker container)
6. Pick a unique name
7. Pick a region (close to home)
14. Review + Create
15. Go to Resource

#### Configure Workflow {.bg-dark .text-light .p-2}

1. In Azure Portal, select All resources and then select the logic app
2. Only one workflow per logic app apparently
5. Select the logic app designer
5. Select a trigger (When message received in service bus queue)
6. Pick the subscription, service bus namespace, access key (you may have to look these up if the portal does not provide drop downs like it should)
6. Pick queue name, queue type (main or dead), polling interval etc.
7. If parsing JSON, you'll need a Parse JSON operation (don't forget the decodebase64 in order to parse the message: add dynamic content, choose expression and enter: decodebase64(triggerbody()?['ContentData']))
8. Add schema if parsing JSON ({ propterties: { message: { type: "string" }, subject: {type: "string"}}, type: "object" })
9. Next operation
1. Add outlook.com "send email"
2. Add creds
3. Configure email
14. Save

#### Configure .net {.bg-dark .text-light .p-2}

```
        static public void SendMessagestring subject, string body) {
 
            string queueUrl = "https://servicename.servicebus.windows.net/queuename";
            string token = GetSasToken(queueUrl, "send", "0jDE9SyjGkpgxxxyyyzzzycFk+8E0SkgWGQ=", TimeSpan.FromDays(1));
            try {
                using (var wc = new WebClient()) {
                    wc.Headers[HttpRequestHeader.Authorization] = token;
                    wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                    wc.UploadString(queueUrl + "/messages", "{ \"subject\": \"" + subject + "\", \"message\": \"" + body.Replace("\\","\\\\") +"\" }");
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                Console.WriteLine("failed to send queue message: " + subject + ": " + body);
            }
        }
 
        public static string GetSasToken(string resourceUri, string keyName, string key, TimeSpan ttl) {
 
            var expiry = GetExpiry(ttl);
            string stringToSign = HttpUtility.UrlEncode(resourceUri) + "\n" + expiry;
            HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
            var signature = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToSign)));
            var sasToken = String.Format(CultureInfo.InvariantCulture, "SharedAccessSignature sr={0}&sig={1}&se={2}&skn={3}",
            HttpUtility.UrlEncode(resourceUri), HttpUtility.UrlEncode(signature), expiry, keyName);
 
            return sasToken;
        }
 
        private static string GetExpiry(TimeSpan ttl) {
            TimeSpan expirySinceEpoch = DateTime.UtcNow - new DateTime(1970, 1, 1) + ttl;
            return Convert.ToString((int)expirySinceEpoch.TotalSeconds);
        }
```

##### add:

```
      - uses: actions/setup-dotnet@v1
        with:
          dotnet-version: 5.0.100
      - run: dotnet restore
      - run: dotnet build --configuration Release --no-restore
      - run: dotnet run --output output
```

##### before:

```
      - name: Build And Deploy
        id: builddeploy
        uses: Azure/static-web-apps-deploy@v1
```

#### Save and enjoy!!  {.bg-dark .text-light .p-2}
