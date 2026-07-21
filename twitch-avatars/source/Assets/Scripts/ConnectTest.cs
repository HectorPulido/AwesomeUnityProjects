using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TwitchBot;
using CielaSpike;
using System.Linq;

public class ConnectTest : UnityBot
{
    public int maxRetry;

    public Avatar avatarPrefabToInstance;
    public Vector2 instancePosition;

    public Explosion explosionPrefab;
    public Candy[] candies;

    public Dictionary<string, Avatar> userAvatars;

    public float coolDownTime = 10f;
    public Dictionary<string, float> coolDowns;


    public bool isBattleRoyale = false;
    public List<Avatar> avatarsForBattleRoyale;
    public Dictionary<string, Avatar> modsAvatars;
    public Dictionary<string, Avatar> subsAvatars;

    public static ConnectTest singleton;

    bool ManageCoolDown(string user)
    {
        if (user == channel) { return true; }

        if (!coolDowns.ContainsKey(user))
        {
            coolDowns.Add(user, Time.time);
            return true;
        }

        if (Time.time - coolDowns[user] > coolDownTime)
        {
            coolDowns[user] = Time.time;
            return true;
        }

        return false;
    }

    void Start()
    {
        if (singleton != null)
        {
            Destroy(this);
            return;
        }
        singleton = this;



        oauth = ConfigData.gameDataConfig.oauthToken;
        channel = ConfigData.gameDataConfig.channelName;
        username = ConfigData.gameDataConfig.username;

        userAvatars = new Dictionary<string, Avatar>();
        modsAvatars = new Dictionary<string, Avatar>();
        subsAvatars = new Dictionary<string, Avatar>();

        coolDowns = new Dictionary<string, float>();
        commands = new Dictionary<string, BotCommand>();
        commands.Add("!jump", new BotCommand((string[] args, Message message) =>
        {
            if (userAvatars.ContainsKey(message.username))
            {
                userAvatars[message.username].Jump();
            }
        }));

        commands.Add("!color", new BotCommand((string[] args, Message message) =>
        {
            if (userAvatars.ContainsKey(message.username))
            {
                try
                {
                    var color = args[1];
                    userAvatars[message.username].SetColor(color);
                }
                catch (System.Exception)
                {
                    print("HUBO UN ERROOOOOOR");
                }
            }
        }));

        commands.Add("!respawn", new BotCommand((string[] args, Message message) =>
        {
            if (message.username != channel)
            {
                return;
            }

            Respawn();
        }));

        commands.Add("!change-avatar", new BotCommand((string[] args, Message message) =>
        {
            userAvatars[message.username].RandomizeSprites();
        }));

        commands.Add("!love", new BotCommand((string[] args, Message message) =>
        {
            var user = args[1].ToLower().Replace("@", "");

            if (userAvatars.ContainsKey(user))
            {
                userAvatars[user].ShowHeart();
                userAvatars[message.username].ShowHeart();
            }
        }));

        commands.Add("!bomb", new BotCommand((string[] args, Message message) =>
        {
            if (!ManageCoolDown(message.username))
            {
                return;
            }

            var explosionPosition = GetRandomPosition();
            var bomb = Instantiate(explosionPrefab, explosionPosition, Quaternion.identity);

            bomb.transform.position = explosionPosition;
            bomb.ActivateBomb();
        }));

        commands.Add("!hiroshima", new BotCommand((string[] args, Message message) =>
        {
            if (message.username != channel)
            {
                return;
            }

            for (int i = 0; i < 5; i++)
            {
                var explosionPosition = GetRandomPosition();
                var bomb = Instantiate(explosionPrefab, explosionPosition, Quaternion.identity);

                bomb.transform.position = explosionPosition;
                bomb.gameObject.SetActive(true);
                bomb.ActivateBomb();
            }
        }));

        commands.Add("!dulces", new BotCommand((string[] args, Message message) =>
        {
            if (!ManageCoolDown(message.username))
            {
                return;
            }

            // random candy
            var thrower = userAvatars[message.username].gameObject;

            var candy = candies[Random.Range(0, candies.Length)];

            var positionToThrow = thrower.transform.position;
            var direction = new Vector2(Random.Range(-1f, 1f), 1);

            var candyInstance = Instantiate(candy, positionToThrow, Quaternion.identity);
            candyInstance.Throw(direction, thrower.gameObject);
        }));

        commands.Add("!battle-royale", new BotCommand((string[] args, Message message) =>
        {
            // TODO, start battle between subs, vips, or a list of users

            if (message.username != channel)
            {
                return;
            }

            avatarsForBattleRoyale.Clear();

            var listOfBattle = userAvatars.Values.ToList().ToArray();
            if (args.Length > 1)
            {
                if (args[1].ToLower() == "mods")
                {
                    listOfBattle = modsAvatars.Values.ToList().ToArray();
                }
                else if (args[1].ToLower() == "subs")
                {
                    listOfBattle = subsAvatars.Values.ToList().ToArray();
                }
            }

            StartBattleRoyale(listOfBattle);
        }));

        commands.Add("!duel", new BotCommand((string[] args, Message message) =>
        {
            if (isBattleRoyale)
            {
                return;
            }

            if (!ManageCoolDown(message.username))
            {
                return;
            }
            var user = args[1].ToLower().Replace("@", "");
            if (!userAvatars.ContainsKey(user))
            {
                return;
            }
            avatarsForBattleRoyale.Clear();

            var avatars = new Avatar[]{
                userAvatars[user], userAvatars[message.username]
            };

            StartBattleRoyale(avatars);
        }));

        whenNewMessage += (Message message) =>
        {
        };

        whenNewSystemMessage += (string message) =>
        {
        };

        whenDisconnect += () =>
        {
        };

        whenStart += () =>
        {
        };

        whenSub += (Message message) =>
        {
        };

        // Evento interesante
        whenNewChater += (Message message) =>
        {
            GenerateAvatar(message.username, message.isMod, message.isSubscriber);
        };

        this.StartCoroutineAsync(StartConnection(maxRetry));
    }

    public void HandleDie(Avatar avatar)
    {
        if (!avatarsForBattleRoyale.Contains(avatar))
        {
            return;
        }

        SendMessageToChat($"{avatar.gameObject.name}, Ha sido eliminado!");


        avatarsForBattleRoyale.Remove(avatar);
        if (avatarsForBattleRoyale.Count == 1)
        {
            SendMessageToChat($"{avatarsForBattleRoyale[0].gameObject.name}, Ha ganado el battle royale!");
            StartCoroutine(WaitForCallback.WaitForSecondsCallback(5f, () => Respawn()));
        }
    }

    public void StartBattleRoyale(Avatar[] avatars)
    {
        foreach (var item in userAvatars)
        {
            item.Value.SetHidden(true);
        }


        avatarsForBattleRoyale.Clear();
        isBattleRoyale = true;
        foreach (var avatar in avatars)
        {
            avatar.SetHidden(false);
            avatar.SetKnife(true);
            avatarsForBattleRoyale.Add(avatar);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, instancePosition * 2);
    }

    [ContextMenu("AddAvatar")]
    public void TestAddAvatar()
    {
        GenerateAvatar("Test " + Random.Range(0, 100).ToString(), false, false);
    }

    private void GenerateAvatar(string username, bool isMod, bool isSubscriber)
    {
        Debug.Log($"{username}, bienvenido al stream!");

        var instanceRandomPosition = GetRandomPosition();
        Avatar go = Instantiate(avatarPrefabToInstance, instanceRandomPosition, Quaternion.identity);

        go.Setup(username);
        go.name = username;

        userAvatars.Add(username, go);

        if (isMod)
        {
            modsAvatars.Add(username, go);
        }

        if (isSubscriber)
        {
            subsAvatars.Add(username, go);
        }

        if (isBattleRoyale)
        {
            go.SetHidden(true);
        }
    }

    private Vector2 GetRandomPosition()
    {
        var instanceRandomPositionX = Random.Range(-instancePosition.x / 2 - transform.position.x, instancePosition.x / 2 - transform.position.x);
        var instanceRandomPositionY = Random.Range(-instancePosition.y / 2 - transform.position.y, instancePosition.y / 2 - transform.position.y);
        var instanceRandomPosition = new Vector2(instanceRandomPositionX, instanceRandomPositionY);

        return instanceRandomPosition;
    }

    private void Respawn()
    {
        isBattleRoyale = false;
        foreach (var item in userAvatars)
        {
            item.Value.SetKnife(false);
            item.Value.SetHidden(false);
            item.Value.transform.position = GetRandomPosition();
            item.Value.SetGhost(false);
        }
    }
}
