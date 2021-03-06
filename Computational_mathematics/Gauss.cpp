// Computational_mathematics.cpp: определяет точку входа для консольного приложения.
#include "stdafx.h"
#include <iostream>
#include <cmath>
#include <ctime>
#include <locale>
#include <iomanip>
using namespace std;
void Search(int size, float **mas);
void Decision(int size, float **mas, int buf);
void Conclusion(int size, float **mas);

void Search(int size, float **mas)
{
	int flag = 1, buf = 0;
	for (int j = 0; j < size; j++)
	{
		for (int i = buf; i < size; i++)
		{
			if (mas[flag][j] < mas[i][j])
			{
				flag = i;
			}
		}
		cout << flag << endl;
		Decision(size, mas, buf);
		buf++;
	}
}

void Decision(int size, float **mas, int buf)
{
	int flag = 0;
	float *save = new float[size + 1];
	for (int i = buf; i < size; i++)
	{
		for (int j = buf; j < size + 1; j++)
		{
			if (buf == i)
			{
				mas[buf][j] = (round((mas[i][j] / mas[i][buf]) * 100) / 100);
			}
			else
			{
				save[j] = mas[buf][j];
				mas[buf][j] = (round((mas[i][j] / mas[i][buf]) * 100)) / 100;
				mas[i][j] = save[j];
			}
		}
	}
	cout << "Exchange\n";
	for (int i = 0; i < size; i++)
	{
		for (int j = 0; j < size + 1; j++)
		{
			if (j == size)
			{
				cout << " | ";
			}
			cout << mas[i][j] << setw(6);
		}
		cout << "\n";
	}
	for (int i = buf; i < size; i++)
	{
		for (int j = 0; j < size + 1; j++)
		{
			if (i != buf)
			{
				mas[i][j] = (round((mas[i][j] - mas[buf][j]) * 100) / 100);
			}
		}
	}
	cout << "Subtraction\n";
	for (int i = 0; i < size; i++)
	{
		for (int j = 0; j < size + 1; j++)
		{
			if (j == size)
			{
				cout << " | ";
			}
			cout << mas[i][j] << setw(6);
		}
		cout << "\n";
	}

	for (int i = 0; i < size; i++)
	{
		for (int j = 0; j < size; j++)
		{
			if (mas[i][j] != 0)
				flag++;
		}
		if (flag == 0)
		{
			cout << "There is no decision\n";
			exit(1);
		}
		if (flag == 1)
		{
			Conclusion(size, mas);
			continue;
		}
		flag = 0;
	}
	delete[]save;
}

void Conclusion(int size, float **mas)
{
	int buf = 0;
	cout << "Conclusion :\n";
	for (int i = 0; i < size; i++)
	{
		for (int j = 0; j < size + 1; j++)
		{

			if (j == size)
			{
				cout << " = " << mas[i][j];
				continue;
			}
			cout << mas[i][j] << " * " << "x" << j + 1 << " + ";

		}
		cout << "\n";
	}
	float *arr = new float[size];
	mas[size - 1][size] = mas[size - 1][size] / mas[size - 1][size - 1];
	for (int j = size - 1; j >= 0; j--)
	{
		arr[j] = mas[j][size];
		for (int i = 0; i < j; i++)
		{
			mas[i][size] = mas[i][size] - mas[i][j] * arr[j];

		}
	}
	for (int i = 0; i < size; i++)
	{
		cout << "x" << i + 1 << " = " << mas[i][size] << endl;
	}
	exit(0);
}

int main()
{
	int size;
	srand(time(0));
	cout << "Enter size massive\n";
	cin >> size;
	float **mas = new float*[size + 1];
	for (int i = 0; i < size + 1; i++)
	{
		mas[i] = new float[size];
	}
	for (int i = 0; i < size; i++)
	{
		for (int j = 0; j < size + 1; j++)
		{
			mas[i][j] = 1 + rand() % 10;
		}
	}
	for (int i = 0; i < size; i++)
	{
		for (int j = 0; j < size + 1; j++)
		{
			cout << mas[i][j] << setw(6);
		}
		cout << "\n";
	}
	Search(size, mas);

	return 0;
}