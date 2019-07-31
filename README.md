# GraphQL QueryLess

Cross library for building GraphQL queries from a class schema.

## Setup

Install GraphQL-QL NuGet package into your PCL/.NetStandard project.

The provided demo uses [this API](https://github.com/saimel/GraphQL-Test-Backend) as backend.

## How to use it

When using GraphQL with Xamarin Forms, it's a good practice to map queries result into a class through a JSON library. Normally the query schema matches the result schema. So, _why to write down the query if the class which you are going to map the result to already contains the schema?_ 

### Defining Mapping Models

The most important step here is to be careful when defining the models you are gonna use to map queries result. You must define your models having in mind possible queries on your client app by using inheritance, so you be able to reuse your models. Next lines show sample models used in the demo.

```C#
public class BaseModel : INotifyPropertyChanged
{
    protected int id;

    public int Id { get => id; set { id = value; OnProperyChanged(nameof(Id)); } }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnProperyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class PlayerListModel : BaseModel
{
    public string Name { get; set; }
}

public class PlayerModel : PlayerListModel
{
    public DateTime BirthDate { get; set; }

    public string BirthPlace { get; set; }

    public string Height { get; set; }

    public int WeightLbs { get; set; }

    public IList<PlayerStatsModel> PlayerStats { get; set; }
}

public class PlayerStatsModel : BaseModel
{
    public int Average { get; set; }

    public string Season { get; set; }

    public string TeamAbbreviation { get; set; }

    public string LeagueAbbreviation { get; set; }

    public int GamesPlayed { get; set; }

    public int AtBat { get; set; }

    public int Hits { get; set; }

    [GraphQLMapping("rbis")]
    public int RBIs { get; set; }

    public int HomeRuns { get; set; }
}
```

### Fetching data from server

This demo project contains these models definition because in one view it has a query asking for all players, and in this view it shows a list of players, needing only `id` and `name` for each player. So `PlayerListModel` is used to generate the query by executing:

```C#
string query = GraphQLQueryLess.BuildQuery<PlayerListModel>("allPlayers");

var players = await GraphQLService.PostQueryAsync<IList<PlayerListModel>>(App.Endpoint, query, "allPlayers");
```

If you decide not to use this query provider, you have to write down the query and your code will look like this:

```C#
string query = $@"{{ allPlayers {{ id name }} }}";
var players = await GraphQLService.PostQueryAsync<IList<PlayerListModel>>(App.Endpoint, query, "allPlayers");
```

As you can see in the example, there is no much difference in the work you have to do. But what if whe are requesting a more complex query?

This demo contains another view: __PlayerDetailsPage__ where user can see several information from a single player, including his statistics from last 3 seasons. Let's see how can you do this.

```C#
var filters = new Dictionary<string, object> { ["id"] = _playerId };
string query = GraphQLQueryLess.BuildQuery<PlayerModel>("player", filters);

var player = await GraphQLService.PostQueryAsync<PlayerModel>(App.Endpoint, query, "player");
```

And let's compare this code above with the one bellow when the query is writen down:

```C#
string query = $@"{{
	player(id: {_playerId} {{
		id
		name
		birthDate
		birthPlace
		height
		weightLbs
		playerStats {{
			season
			teamAbbreviation
			leagueAbbreviation
			gamesPlayed
			atBat
			hits
			rbis
			homeRuns
			average
		}}
	}}
}}";

var player = await GraphQLService.PostQueryAsync<PlayerModel>(App.Endpoint, query, "player");
```

As you can see, it is so much confortable to do it by using the query builder. Not only because you don't have to write down the query, you will also reduce the possibilities of typos in your query, leading to errors.

#### Custom attributes

Some times you may have a property in the response that doesn't match with your model schema. In this sample it happens with `RBIs`. The serialization library expects to map it to a property named `rBIs` (_it is case sensitive_) and it will lead to an exception. 

The solution for this inconvenient is to use `GraphQLMappingAttribute` as you can see at `RBIs` property in `PlayerStatsModel`.

### Mutations

Mutations are built very similar, requiring the `TResult` for building the query for mapping the response and the variables required for the mutation. Here is an expample for adding the statistics of one season for one player:

```C#
var parameters = new List<MutationParameters>();
parameters.Add(new MutationParameters { Name = "stats", Type = "StatisticInput!", Content = StatsModel });

var mutation = GraphQLQueryLess.BuildMutation<PlayerStatsModel>("createStats", parameters);

var response = await GraphQLService.PostAsync<PlayerStatsModel>(App.Endpoint, mutation, "createStats");
```

Let's see how it will look if you write down your mutation by hand:

```C#
string mutation = $@"
mutation {{
	createStats(
		stats: {{
			playerId: 4,
			teamId: 3,
			seasonId: 2,
			gamesPlayed: 150,
			atBat: 500,
			hits: 125,
			homeRuns: 30,
			rbis: 95
		}}
	) {{
		season
		teamAbbreviation
		leagueAbbreviation
		gamesPlayed
		atBat
		hits
		rbis
		homeRuns
		average
	}}
}}";

var response = await GraphQLService.PostAsync<PlayerStatsModel>(App.Endpoint, mutation, "createStats");
```

Again, there is a difference there.

__Important:__ When passing variables to mutations, it's also recommended to be careful when defining properties name into the models you will use. See __JsonPropertyAttribute__ at `RBIs` property in `CreateStatsModel` bellow.

```C#
public class CreateStatsModel
{
    public int PlayerId { get; set; }

    public int TeamId { get; set; }

    public int SeasonId { get; set; }

    public int GamesPlayed { get; set; }

    public int AtBat { get; set; }

    public int Hits { get; set; }

    public int HomeRuns { get; set; }

    [JsonProperty("rbis")]
    public int RBIs { get; set; }

    public CreateStatsModel(int playerId)
    {
        PlayerId = playerId;
    }
}
```


