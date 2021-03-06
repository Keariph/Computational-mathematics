#include "stdafx.h"
#include <iostream>
#include <ctime>
#include <cmath>
#include <fstream>
using namespace std;
void File(float **mas, int size);
void Random(float**mas, int size);
void Smashing(float **mas, int size);
void Max(float ** masB, float *masC, int size);
void MPI(float **masB, float *masC, int size, int n);
void Seidel(float *masC, float **masB, int size, int n);
void Iteration(float maxB, float maxC, float **masB, float *masC, int size);
void Output(float **masB, int size);
void OutputX(float **con, int size);
void Output(float *masC, int size);
//Правильно
void File(float ** mas, int size)
{
	ifstream fin("C:\\Users\\Evil Spirit\\source\\repos\\Computational_Mathematics\\array.txt");
	if (!fin.is_open())
		cout << " File can not open\n";
	else
	{
		for (int i = 0; i < size; i++)
		{
			for (int j = 0; j < size + 1; j++)
			{
				fin >> mas[i][j];
			}
		}
		Smashing(mas, size);
	}
}
//Правильно
void Random(float **mas, int size)//Заполнение массива
{
	srand(time(NULL));
	for (int i = 0; i < size; i++)
	{
		for (int j = 0; j < size + 1; j++)
		{
			mas[i][j] = rand() % 10 + 1;
		}
	}
	cout << "Inital array\n";
	OutputX(mas, size);
	Smashing(mas, size);
}
//Правильно
void Smashing(float **mas, int size)//Разбивание на три матрицы
{
	float **masA = new float*[size] {0};
	float **masB = new float*[size] {0};
	float *masC = new float[size] {0};
	for (int i = 0; i < size; i++)
	{
		masA[i] = new float[size];
		masB[i] = new float[size];
	}
	for (int i = 0; i < size; i++)
	{
		for (int j = 0; j < size; j++)
		{
			if (i == j)
			{
				masA[i][j] = 1;
				masB[i][j] = 0;
			}
			else masB[i][j] = round((mas[i][j] / mas[i][i])*100)/100;
		}
	}
	cout << "Array B" << endl;
	Output(masB, size);
	for (int i = 0; i < size; i++)
	{
		masC[i] = round((mas[i][size]/mas[i][i])*100)/100;
	}
	cout << "Array C" << endl;
	for (int i = 0; i < size; i++)
	{
		cout <<masC[i] << endl;
	}
	Max(masB, masC, size);
}
//Правильно
void Max(float ** masB, float *masC, int size)//Нахождение max
{
	float maxB = 0;
	float *buf = new float[size]{0};
	float maxC = 0;
	for (int i = 0; i < size; i++)
	{
		for (int j = 0; j < size; j++)
		{
			if (buf[i] < 0)
				buf[i] = buf[i] * (-1);
			buf[i] = masB[i][j] + buf[i];
		}
	}
	for (int i = 0; i < size; i++)
	{
		if (masC[i] > maxC) 
		{
			if (masC[i] < 0)
			{
				masC[i] = masC[i] * (-1);
			}
			maxC = masC[i];
		}
		if(buf[i]>maxB)
		maxB = buf[i];
	}
	cout << "||B|| = " << maxB<<endl;
	cout << "||C|| = " << maxC << endl;
	Iteration(maxB, maxC, masB, masC, size);
}
//Решает
void MPI(float **masB, float *masC, int size, int n)//Метод простых итераций
{
	float *buf = new float[size] {0};
	float *x = new float[size] {0};
	float sum = 0;
	for (int k = 0; k < n; k++)
	{
		for (int i = 0; i < size; i++)
		{
			for (int j = 0; j < size; j++)
			{
				sum = (masB[i][j] * buf[j]) + sum;
			}
			x[i] = masC[i] - sum;
			sum = 0;
		}
		for (int i = 0; i < size; i++)
		{
			buf[i] = x[i];
		}
		cout << "Iteratoin " << k + 1 << endl;
		Output(x, size);
	}
}

void Seidel(float *masC, float **masB, int size, int n)//Метод Зейделя
{
	float *x = new float[size] {0};
	float sum = 0;
	for (int k = 0; k < n; k++)
	{
		for (int i = 0; i < size; i++)
		{
			for (int j = 0; j < size; j++)
			{
				sum = (masB[i][j] * x[j]) + sum;
			}
			x[i] = masC[i] - sum;

		}
		cout << "Iteration " << k + 1 << endl;
		Output(x, size);
	}
}
//Правильно
void Iteration(float maxB, float maxC, float **masB, float *masC, int size)//Количество шагов
{
	int enter;
	float logB = round((log(maxB))*100)/100;
	float B =round((1 - maxB)*1000)/1000;
	float B1 = round((B / maxC)*100)/100;
	if (B1 < 0)
		B1 = B1 * (-1);
	float logB1 = round((log(B1))*100)/100;
	int n = round((logB1 / logB)*100)/100;
	if (n < 0)
		n = n * (-1);
	n += 1;
	cout << "N = " << n << endl;
	cout << "Select a method\n1. MPI\t2.method Seidel\n";
	cin >> enter;
	switch (enter)
	{
	case 1:
		MPI(masB, masC, size, n);
		break;
	case 2:
		Seidel(masC, masB, size, n);
	default:
		cout << "Incorect input\n";
		break;
	}
}
//Правильно
void Output(float ** masB, int size)
{
	for (int i = 0; i < size; i++)
	{
		for (int j = 0; j < size; j++)
		{
			cout << masB[i][j] << " ";
		}
		cout << endl;
	}
}
//Правильно
void OutputX(float ** con, int size)
{
	for (int i = 0; i < size; i++)
	{
		for (int j = 0; j < size + 1; j++)
		{
			cout << con[i][j] << " ";
		}
		cout << endl;
	}
}
//Правильно
void Output(float * masC, int size)
{
	for (int i = 0; i < size; i++)
	{
		cout << "x" << i + 1 << " = " << masC[i] << endl;
	}
}

int main()
{
	int size, enter;
	cout << "Enter the size array\n";
	cin >> size;
	float **mas = new float*[size+1];
	for (int i = 0; i < size+1; i++)
	{
		mas[i] = new float[size];
	}
	cout << "Select input method\n1. Manually\t2. Randomly\n";
	cin >> enter;
	switch (enter)
	{
	case 1:
		File(mas, size);
		break;
	case 2:
		Random(mas, size);
		break;
	default:
		cout << "Incotect input\n";
		break;
	}

    return 0;
}

