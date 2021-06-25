using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MakeMaze : MonoBehaviour
{
	public int mazeSizeX = 11;
	public int mazeSizeY = 11;

	public struct Position
	{
		public int x;
		public int y;

		public Position(int x, int y)
        {
			this.x = x;
			this.y = y;
        }
	};

    class Mtable
    {
		public int x;
		public int y;

		public Mtable(int x, int y)
        {
			this.x = x;
			this.y = y;
        }
	}

	Mtable[] Move = {
		new Mtable (0, 1),
		new Mtable (1, 0),
		new Mtable (0, -1),
		new Mtable (-1, 0),
	};

	Stack<Position> mazeStack = new Stack<Position>();
	Position currentPosition = new Position(0, 0);

	int dir = 0;
	int[] dirCheck = { 0, 0, 0, 0 };

	private GameObject[,] stages = new GameObject[11, 11];

	public GameObject stage;

	public List<Vector3> enablePosition = new List<Vector3>();

	int[,] maze = new int[11, 11]
	{
		{1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
		{1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
		{1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
		{1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
		{1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
		{1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
		{1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
		{1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
		{1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
		{1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
		{1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
	};

	private void Awake()
    {
		int mazeCheckSizeX = (mazeSizeX - 1) / 2;
		int mazeCheckSizeY = (mazeSizeY - 1) / 2;

		int[,] mazeCheck = new int[mazeCheckSizeX, mazeCheckSizeY];

		for (int i = 0; i < stage.transform.childCount; i++)
        {
			stages[i / 11, i % 11] = stage.transform.GetChild(i).gameObject;
        }

		mazeStack.Push(currentPosition);
		mazeCheck[currentPosition.x, currentPosition.y] = 1;

		while (mazeStack.Count() != 0)
		{
			while (true)
			{
				dir = Random.Range(0, 4);

				if (dirCheck[dir] == 0)
				{
					dirCheck[dir] = 1;
					int moveX = Move[dir].x;
					int moveY = Move[dir].y;

					if (currentPosition.x + moveX >= 0 && currentPosition.x + moveX < mazeCheckSizeX && currentPosition.y + moveY >= 0 && currentPosition.y + moveY < mazeCheckSizeY && mazeCheck[currentPosition.x + moveX, currentPosition.y + moveY] == 0)
					{
						maze[currentPosition.x * 2 + 1, currentPosition.y * 2 + 1] = 0;
						maze[(currentPosition.x * 2 + 1) + moveX * 2, (currentPosition.y * 2 + 1) + moveY * 2] = 0;
						maze[(currentPosition.x * 2 + 1) + moveX, (currentPosition.y * 2 + 1) + moveY] = 0;
						currentPosition.x += moveX;
						currentPosition.y += moveY;
						mazeStack.Push(currentPosition);
						mazeCheck[currentPosition.x, currentPosition.y] = 1;
						break;
					}
				}
				else
				{
					bool isDir = false;

					for (int i = 0; i < 4; i++)
					{
						if (dirCheck[i] == 0)
						{
							isDir = true;
							break;
						}
					}

					if (isDir == false)
					{
						Position temp = mazeStack.Pop();
						currentPosition.x = temp.x;
						currentPosition.y = temp.y;
						break;
					}
				}
			}

			for (int i = 0; i < 4; i++)
			{
				dirCheck[i] = 0;
			}
		}

		for (int i = 0; i < 11; i++)
        {
			for (int j = 0; j < 11; j++)
            {
				if (maze[i, j] == 0)
                {
					stages[i, j].SetActive(false);
					enablePosition.Add(stages[i, j].transform.position);
                }
            }
        }

		stage.transform.GetChild(11).gameObject.SetActive(false);
		stage.transform.GetChild(109).gameObject.SetActive(false);
	}
}
