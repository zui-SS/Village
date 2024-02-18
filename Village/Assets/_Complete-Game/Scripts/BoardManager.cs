using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic; 		//Allows us to use Lists.
using Random = UnityEngine.Random;
using Unity.VisualScripting; 		//Tells Random to use the Unity Engine random number generator.

namespace Completed
{
	
	public class BoardManager : MonoBehaviour
	{
		public class Count
		{
			public int minimum; 			//Minimum value for our Count class.
			public int maximum; 			//Maximum value for our Count class.
			
			
			//Assignment constructor.
			public Count (int min, int max)
			{
				minimum = min;
				maximum = max;
            }
        }

        // タイル種類.
        public enum E_TILE_TYPE { 
            E_NONE= 0
            , E_TYLE_FLOOR
            , E_TYLE_WALL
            , E_TYLE_BREAKAQBLE_WALL
            , E_TYLE_ENEMY
            , E_TYLE_FOOD
            , E_TYLE_DRINK
            , E_TYLE_EXIT
            , E_TYLE_NUM 
        }

        public class Range {
            public Vector2Int Start;
            public Vector2Int End;

            public Range(Vector2Int start, Vector2Int end)
            {
                Start = start;
                End = end;
            }

            public Range(int startX, int startY, int endX, int endY)
            {
                Start = new Vector2Int(startX, startY);
                End = new Vector2Int(endX, endY);
            }

            public Range()
            {
                Start = new Vector2Int(0, 0);
                End = new Vector2Int(0, 0);
            }

            public int GetWidthX()
            {
                return End.x - Start.x;
            }

            public int GetWidthY()
            {
                return End.y - Start.y;
            }
        }


        /**
		 * 定義移植．
		 */
        private const int MINIMUM_RANGE_WIDTH = 6;
        private const int MINIMUM_ENEMY_NUM = 3;

        private int mapSizeX = 30;
        private int mapSizeY = 30;
        private int maxRoom = 6;

        private List<Range> roomList = new List<Range>();
        private List<Range> rangeList = new List<Range>();
        private List<Range> passList = new List<Range>();
        private List<Range> roomPassList = new List<Range>();

        private bool shouldInitializePlayer = true;                                           //Prefab to player.
        [SerializeField] private Player player;
        private DebugFunc debug;
		private Transform boardHolder;									//A variable to store a reference to the transform of our Board object.
        
        public Vector2Int PlayerPos;
        public Count wallCount = new Count (5, 9);						//Lower and upper limit for our random number of walls per level.
		public Count foodCount = new Count (1, 3);                      //Lower and upper limit for our random number of food items per level.
		public GameObject exit;											//Prefab to spawn for exit.
        /**
		 * ここまで.
		 */

        
        
        public GameObject[] floorTiles;									//Array of floor prefabs.
		public GameObject[] wallTiles;									//Array of wall prefabs.
		public GameObject[] foodTiles;									//Array of food prefabs.
		public GameObject[] enemyTiles;									//Array of enemy prefabs.
		public GameObject[] outerWallTiles;								//Array of outer tile prefabs.
		private List <Vector3> gridPositions = new List <Vector3> ();	//A list of possible locations to place tiles.
		
		
		
		//Clears our list gridPositions and prepares it to generate a new board.
		void InitialiseList ()
		{
			//Clear our list gridPositions.
			gridPositions.Clear ();
			
			//Loop through x axis (this.mapSizeX).
			for(int x = 0; x < this.mapSizeX; x++)
			{
				//Within each column, loop through y axis (this.mapSizeY).
				for(int y = 0; y < this.mapSizeY; y++)
				{
					//At each index add a new Vector3 to our list with the x and y coordinates of that position.
					gridPositions.Add (new Vector3(x, y, 0f));
				}
			}
		}

        // ゲーム開始時にだけやる処理.
        void Initialize(int level)
        {
            shouldInitializePlayer = true;
            roomList.Clear();
            rangeList.Clear();
            passList.Clear();
            roomPassList.Clear();

            // 2階以降は削除を行う.
            // すべての子オブジェクトを取得
            if (GameObject.Find("Board") != null)
            {
                boardHolder = GameObject.Find("Board").transform;
                foreach (Transform n in boardHolder)
                {
                    GameObject.Destroy(n.gameObject);
                }
                GameObject.Destroy(boardHolder.gameObject);
            }
            BoardSetup();
        }

        //Sets up the outer walls and floor (background) of the game board.
        void BoardSetup ()
		{
			this.boardHolder = new GameObject ("Board").transform;
			
			//Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
			for(int x = 0; x < this.mapSizeX; x++)
			{
				//Loop along y axis, starting from -1 to place floor or outerwall tiles.
				for(int y = 0; y < this.mapSizeY; y++)
				{
					//Choose a random tile from our array of floor tile prefabs and prepare to instantiate it.
                    // とりあえず全て壁で埋める.
					GameObject toInstantiate = floorTiles[Random.Range (0, floorTiles.Length - 1)];
					
					//Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
					GameObject instance =
						Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					
					//Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
					instance.transform.SetParent (this.boardHolder);
				}
			}
            

        }
		
        // 範囲からランダムな位置に任意のタイルを置く.		
		Vector2Int RandomPosition (Range range)
		{
			//Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in our List gridPositions.
			int randomIndex = Random.Range (0, gridPositions.Count);

            //Declare a variable of type Vector3 called randomPosition, set it's value to the entry at randomIndex from our List gridPositions.
            int randomX = Random.Range(range.Start.x, range.End.x);
            int randomY = Random.Range(range.Start.y, range.End.y);

            Vector2Int randomPosition = new Vector2Int(randomX, randomY);

			//Return the randomly selected Vector3 position.
			return randomPosition;
		}

        //RandomPosition returns a random position from our list gridPositions.
        Range RandomRoom()
        {
            //Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in our List gridPositions.
            int randomIndex = Random.Range(0, roomList.Count);

            //Declare a variable of type Vector3 called randomPosition, set it's value to the entry at randomIndex from our List gridPositions.
            return roomList[randomIndex];
        }


        //LayoutObjectAtRandom accepts an array of game objects to choose from along with a minimum and maximum range for the number of objects to create.
        void LayoutObjectAtRandom (int[,] map, E_TILE_TYPE type, Range range, int minimum, int maximum)
		{
			//Choose a random number of objects to instantiate within the minimum and maximum limits
			int objectCount = Random.Range (minimum, maximum);
			
			//Instantiate objects until the randomly chosen limit objectCount is reached
			for(int i = 0; i < objectCount; i++)
			{
				//Choose a position for randomPosition by getting a random position from our list of available Vector3s stored in gridPosition
				Vector2Int randomPosition = RandomPosition(range);
                if(type == E_TILE_TYPE.E_TYLE_EXIT)
                {
                    Debug.Log("Exit:" + randomPosition.ToString());
                }
                map[(int)randomPosition.x, (int)randomPosition.y] = (int)type;
            }
		}
		
		
		//SetupScene initializes our level and calls the previous functions to lay out the game board
		public void SetupScene (int level)
		{
            // 初期化.
            Initialize(level);

            this.debug = new DebugFunc();

            ////Reset our list of gridpositions.
            //InitialiseList ();

            ////Instantiate a random number of wall tiles based on minimum and maximum, at randomized positions.
            //LayoutObjectAtRandom (wallTiles, wallCount.minimum, wallCount.maximum);

            ////Instantiate a random number of food tiles based on minimum and maximum, at randomized positions.
            //LayoutObjectAtRandom (foodTiles, foodCount.minimum, foodCount.maximum);

            // レベルに応じた敵数.
            int enemyCount = (int)Mathf.Log(level + MINIMUM_ENEMY_NUM, 2f);

            //Instantiate a random number of enemies based on minimum and maximum, at randomized positions.
            //LayoutObjectAtRandom (enemyTiles, enemyCount, enemyCount);

            ////Instantiate the exit tile in the upper right hand corner of our game board
            //Instantiate (exit, new Vector3 (this.mapSizeX - 1, this.mapSizeY - 1, 0f), Quaternion.identity);

            // マップを敷く.
            int[,] map = GenerateMap(this.maxRoom, enemyCount);
            Debug.Log("PutMap level" + level.ToString());
            debug.PrintMap(map, this.mapSizeX, this.mapSizeY);
            PutMap(map);
        }

        // マップを敷く.
        public void PutMap(int[,] map)
        {
            for (int x = 0; x < this.mapSizeX; x++)
            {
                //Loop along y axis, starting from -1 to place floor or outerwall tiles.
                for (int y = 0; y < this.mapSizeY; y++)
                {
                    int eTypeType = map[x, y];
                    GameObject instance = Instantiate(GetTyle((E_TILE_TYPE)map[x, y]), new Vector3(x, y, 0f), Quaternion.identity);
                    instance.transform.SetParent(this.boardHolder);
                }                
            }
        }

        // タイルを取得.
        public GameObject GetTyle(E_TILE_TYPE　eTyleType)
        {
            switch (eTyleType)
            {
                case E_TILE_TYPE.E_TYLE_FLOOR:
                    return floorTiles[MathUtils.GetRandomInt(0, floorTiles.Length - 1)];
                case E_TILE_TYPE.E_TYLE_WALL:
                    return outerWallTiles[MathUtils.GetRandomInt(0, outerWallTiles.Length - 1)];
                case E_TILE_TYPE.E_TYLE_BREAKAQBLE_WALL:
                    return wallTiles[MathUtils.GetRandomInt(0, wallTiles.Length - 1)];
                case E_TILE_TYPE.E_TYLE_ENEMY:
                    return enemyTiles[MathUtils.GetRandomInt(0, enemyTiles.Length - 1)];
                case E_TILE_TYPE.E_TYLE_FOOD:
                case E_TILE_TYPE.E_TYLE_DRINK:
                    return foodTiles[MathUtils.GetRandomInt(0, foodTiles.Length - 1)];
                case E_TILE_TYPE.E_TYLE_EXIT:
                    return exit;
                default:
                    return outerWallTiles[MathUtils.GetRandomInt(0, outerWallTiles.Length - 1)];
            }
        }

        // マップ作製.
        public int[,] GenerateMap (int maxRoom, int enemyNum)
        {
            int[,] map = new int[this.mapSizeX, this.mapSizeY];

            CreateRange(maxRoom);
            CreateRoom();

            // ここまでの結果を一度配列に反映する
            // 通路.
            foreach (Range pass in passList)
            {
                for (int x = pass.Start.x; x <= pass.End.x; x++)
                {
                    for (int y = pass.Start.y; y <= pass.End.y; y++)
                    {
                        map[x, y] = (int)E_TILE_TYPE.E_TYLE_FLOOR;
                    }
                }
            }
            // 部屋までの道.
            foreach (Range roomPass in roomPassList)
            {
                for (int x = roomPass.Start.x; x <= roomPass.End.x; x++)
                {
                    for (int y = roomPass.Start.y; y <= roomPass.End.y; y++)
                    {
                        map[x, y] = (int)E_TILE_TYPE.E_TYLE_FLOOR;
                    }
                }
            }
            // 部屋.
            int enemyCount = enemyNum;
            Range playerRoom = RandomRoom();
            Range exitRoom = RandomRoom();
            foreach (Range room in roomList)
            {
                // プレイヤー.
                if (playerRoom.Equals(room))
                {
                    Vector2Int randomPosition = RandomPosition(room);
                    if (player != null)
                    {
                        player.SetPos(new Vector3(randomPosition.x, randomPosition.y, 0f));
                    }
                    PlayerPos = new Vector2Int(randomPosition.x, randomPosition.y);
                    shouldInitializePlayer = false;
                }

                // 出口.
                if (exitRoom.Equals(room))
                {
                    LayoutObjectAtRandom(map, E_TILE_TYPE.E_TYLE_EXIT, room, 1, 1);
                }

                // 回復アイテム.
                LayoutObjectAtRandom(map, E_TILE_TYPE.E_TYLE_FOOD, room, foodCount.minimum, foodCount.maximum + 1);

                // 敵
                LayoutObjectAtRandom(map, E_TILE_TYPE.E_TYLE_ENEMY, room, 0, enemyNum);

                for (int x = room.Start.x; x <= room.End.x; x++)
                {
                    for (int y = room.Start.y; y <= room.End.y; y++)
                    {
                        // 何も入っていなければ床を置く.
                        if(map[x, y] == (int)E_TILE_TYPE.E_NONE)
                        {
                            map[x, y] = (int)E_TILE_TYPE.E_TYLE_FLOOR;
                        }
                    }
                }
            }
            // 余分な通路を削除.
            TrimPassList(ref map);

            return map;
        }

        public void CreateRange(int maxRoom)
        {
            // 区画のリストの初期値としてマップ全体を入れる
            Vector2Int start = new Vector2Int(0,0);
            Vector2Int end = new Vector2Int(this.mapSizeX - 1, this.mapSizeY - 1);
            rangeList.Add(new Range(start, end));

            bool isDevided;
            do
            {
                // 縦 → 横 の順番で部屋を区切っていく。一つも区切らなかったら終了
                isDevided = DevideRange(false);
                isDevided = DevideRange(true) || isDevided;

                // もしくは最大区画数を超えたら終了
                if (rangeList.Count >= maxRoom)
                {
                    break;
                }
            } while (isDevided);

        }

        public bool DevideRange(bool isVertical)
        {
            bool isDevided = false;

            // 区画ごとに切るかどうか判定する
            List<Range> newRangeList = new List<Range>();
            foreach (Range range in rangeList)
            {
                // これ以上分割できない場合はスキップ
                if (isVertical && range.GetWidthY() < MINIMUM_RANGE_WIDTH * 2 + 1)
                {
                    continue;
                }
                else if (!isVertical && range.GetWidthX() < MINIMUM_RANGE_WIDTH * 2 + 1)
                {
                    continue;
                }

                System.Threading.Thread.Sleep(1);

                // 40％の確率で分割しない
                // ただし、区画の数が1つの時は必ず分割する
                if (rangeList.Count > 1 && MathUtils.RandomJadge(0.4f))
                {
                    continue;
                }

                // 長さから最少の区画サイズ2つ分を引き、残りからランダムで分割位置を決める
                int length = isVertical ? range.GetWidthY() : range.GetWidthX();
                int margin = length - MINIMUM_RANGE_WIDTH * 2;
                int baseIndex = isVertical ? range.Start.y : range.Start.x;
                int devideIndex = baseIndex + MINIMUM_RANGE_WIDTH + MathUtils.GetRandomInt(1, margin) - 1;

                // 分割された区画の大きさを変更し、新しい区画を追加リストに追加する
                // 同時に、分割した境界を通路として保存しておく
                Range newRange = new Range();
                if (isVertical)
                {
                    passList.Add(new Range(range.Start.x, devideIndex, range.End.x, devideIndex));
                    newRange = new Range(range.Start.x, devideIndex + 1, range.End.x, range.End.y);
                    range.End.y = devideIndex - 1;
                }
                else
                {
                    passList.Add(new Range(devideIndex, range.Start.y, devideIndex, range.End.y));
                    newRange = new Range(devideIndex + 1, range.Start.y, range.End.x, range.End.y);
                    range.End.x = devideIndex - 1;
                }

                // 追加リストに新しい区画を退避する。
                newRangeList.Add(newRange);

                isDevided = true;
            }

            // 追加リストに退避しておいた新しい区画を追加する。
            rangeList.AddRange(newRangeList);

            return isDevided;
        }

        private void CreateRoom()
        {
            // 部屋のない区画が偏らないようにリストをシャッフルする
            rangeList.Sort((a, b) => MathUtils.GetRandomInt(0, 1) - 1);

            // 1区画あたり1部屋を作っていく。作らない区画もあり。
            foreach (Range range in rangeList)
            {
                System.Threading.Thread.Sleep(1);
                // 30％の確率で部屋を作らない
                // ただし、最大部屋数の半分に満たない場合は作る
                if (roomList.Count > maxRoom / 2 && MathUtils.RandomJadge(0.3f))
                {
                    continue;
                }

                // 猶予を計算
                int marginX = range.GetWidthX() - MINIMUM_RANGE_WIDTH + 1;
                int marginY = range.GetWidthY() - MINIMUM_RANGE_WIDTH + 1;

                // 開始位置を決定
                int randomX = MathUtils.GetRandomInt(1, marginX);
                int randomY = MathUtils.GetRandomInt(1, marginY);

                // 座標を算出
                int startX = range.Start.x + randomX;
                int endX = range.End.x - MathUtils.GetRandomInt(0, (marginX - randomX)) - 1;
                int startY = range.Start.y + randomY;
                int endY = range.End.y - MathUtils.GetRandomInt(0, (marginY - randomY)) - 1;

                // 部屋リストへ追加
                Range room = new Range(startX, startY, endX, endY);
                roomList.Add(room);

                // 通路を作る
                CreatePass(range, room);
            }
        }

        private void CreatePass(Range range, Range room)
        {
            List<int> directionList = new List<int>();
            if (range.Start.x != 0)
            {
                // Xマイナス方向
                directionList.Add(0);
            }
            if (range.End.x != this.mapSizeX - 1)
            {
                // Xプラス方向
                directionList.Add(1);
            }
            if (range.Start.y != 0)
            {
                // Yマイナス方向
                directionList.Add(2);
            }
            if (range.End.y != this.mapSizeY - 1)
            {
                // Yプラス方向
                directionList.Add(3);
            }

            // 通路の有無が偏らないよう、リストをシャッフルする
            directionList.Sort((a, b) => MathUtils.GetRandomInt(0, 1) - 1);

            bool isFirst = true;
            foreach (int direction in directionList)
            {
                System.Threading.Thread.Sleep(1);
                // 80%の確率で通路を作らない
                // ただし、まだ通路がない場合は必ず作る
                if (!isFirst && MathUtils.RandomJadge(0.8f))
                {
                    continue;
                }
                else
                {
                    isFirst = false;
                }

                // 向きの判定
                int random;
                switch (direction)
                {
                    case 0: // Xマイナス方向
                        random = room.Start.y + MathUtils.GetRandomInt(1, room.GetWidthY()) - 1;
                        roomPassList.Add(new Range(range.Start.x, random, room.Start.x - 1, random));
                        break;

                    case 1: // Xプラス方向
                        random = room.Start.y + MathUtils.GetRandomInt(1, room.GetWidthY()) - 1;
                        roomPassList.Add(new Range(room.End.x + 1, random, range.End.x, random));
                        break;

                    case 2: // Yマイナス方向
                        random = room.Start.x + MathUtils.GetRandomInt(1, room.GetWidthX()) - 1;
                        roomPassList.Add(new Range(random, range.Start.y, random, room.Start.y - 1));
                        break;

                    case 3: // Yプラス方向
                        random = room.Start.x + MathUtils.GetRandomInt(1, room.GetWidthX()) - 1;
                        roomPassList.Add(new Range(random, room.End.y + 1, random, range.End.y));
                        break;
                }
            }

        }

        private void TrimPassList(ref int[,] map)
        {
            // どの部屋通路からも接続されなかった通路を削除する
            for (int i = passList.Count - 1; i >= 0; i--)
            {
                Range pass = passList[i];

                bool isVertical = pass.GetWidthY() > 1;

                // 通路が部屋通路から接続されているかチェック
                bool isTrimTarget = true;
                if (isVertical)
                {
                    int x = pass.Start.x;
                    for (int y = pass.Start.y; y <= pass.End.y; y++)
                    {
                        if (map[x - 1, y] == 1 || map[x + 1, y] == 1)
                        {
                            isTrimTarget = false;
                            break;
                        }
                    }
                }
                else
                {
                    int y = pass.Start.y;
                    for (int x = pass.Start.x; x <= pass.End.x; x++)
                    {
                        if (map[x, y - 1] == 1 || map[x, y + 1] == 1)
                        {
                            isTrimTarget = false;
                            break;
                        }
                    }
                }

                // 削除対象となった通路を削除する
                if (isTrimTarget)
                {
                    passList.Remove(pass);

                    // マップ配列からも削除
                    if (isVertical)
                    {
                        int x = pass.Start.x;
                        for (int y = pass.Start.y; y <= pass.End.y; y++)
                        {
                            map[x, y] = 0;
                        }
                    }
                    else
                    {
                        int y = pass.Start.y;
                        for (int x = pass.Start.x; x <= pass.End.x; x++)
                        {
                            map[x, y] = 0;
                        }
                    }
                }
            }

            // 外周に接している通路を別の通路との接続点まで削除する
            // 上下基準
            for (int x = 0; x < this.mapSizeX - 1; x++)
            {
                if (map[x, 0] == 1)
                {
                    for (int y = 0; y < this.mapSizeY; y++)
                    {
                        if (map[x - 1, y] == 1 || map[x + 1, y] == 1)
                        {
                            break;
                        }
                        map[x, y] = 0;
                    }
                }
                if (map[x, this.mapSizeY - 1] == 1)
                {
                    for (int y = this.mapSizeY - 1; y >= 0; y--)
                    {
                        if (map[x - 1, y] == 1 || map[x + 1, y] == 1)
                        {
                            break;
                        }
                        map[x, y] = 0;
                    }
                }
            }
            // 左右基準
            for (int y = 0; y < this.mapSizeY - 1; y++)
            {
                if (map[0, y] == 1)
                {
                    for (int x = 0; x < this.mapSizeY; x++)
                    {
                        if (map[x, y - 1] == 1 || map[x, y + 1] == 1)
                        {
                            break;
                        }
                        map[x, y] = 0;
                    }
                }
                if (map[this.mapSizeX - 1, y] == 1)
                {
                    for (int x = this.mapSizeX - 1; x >= 0; x--)
                    {
                        if (map[x, y - 1] == 1 || map[x, y + 1] == 1)
                        {
                            break;
                        }
                        map[x, y] = 0;
                    }
                }
            }
        }
        public bool IsAlreadySetPlayer()
        {
            return !this.shouldInitializePlayer;
        }
    }

}
