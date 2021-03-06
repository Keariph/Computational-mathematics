// Newton.cpp: определяет точку входа для консольного приложения.
//

#include "stdafx.h"
#include <iostream>
using namespace std;
/*----------------Функции-----------------------*/
double F(int i, double x, double y)
{
	if (i == 0)
	{
		return 2 * pow(x, 2) + pow(y, 2) - 8;
	}
	if (i == 1)
	{
		
		return x / y - 3;;
	}
}
double** FF(double **FF, double x, double y)		
{
	FF[0][0] = 4 * x;
	FF[0][1] = 1 / y;
	FF[1][0] = 2 * y;
	FF[1][1] = (-1) * (x / pow(y, 2));
	return FF;
}
 
//-------------Основные операции ---------------
double** Reverse(double **mas, int size)
{
	double A;
	double **min = new double*[size];
	for (int i = 0; i < size; i++)
	{
		min[i] = new double[size];
	}
	double **T = new double*[size];
	for (int i = 0; i < size; i++)
	{
		T[i] = new double[size];
	}
	double **rev = new double*[size];
	for (int i = 0; i < size; i++)
	{
		rev[i] = new double[size];
	}
	//Определитель
	A = (mas[0][0] * mas[size-1][size-1]) - (mas[0][size-1] * mas[size-1][0]);
	if (A == 0)
	{
		cout << "There is not inverse matrix\n";
		exit(0);
	}
	//Миноры
	for (int i = 0; i < size; i++)
	{
		for (int j = 0; j < size; j++)
		{
			min[i][j] = mas[size - i - 1][size - j - 1] * pow(-1, i + j);
		}
	}
	//Транспонированная матрица
	for (int i = 0; i < size; i++)
	{
		for (int j = 0; j < size; j++)
		{
			T[i][j] = min[j][i];
			
		}
	}//Обратная
	for (int i = 0; i < size; i++)
	{
		for (int j = 0; j < size; j++)
		{
			rev[j][i] = T[i][j] * 1 / A;
		}
	
	}
	return rev;
}
void Output(int k, int size,double **Jacobi, double **Revers, double *X)
{
	cout << "Iteration " << k << endl;
	cout << "Jacobi arrey \n-------------------------------\n";
	for (int i = 0; i < size; i++)
	{
		for (int j = 0; j < size; j++)
		{
			cout << Jacobi[i][j] << " ";
		}
		cout << endl;
	}
	cout << "Revers arrey \n-------------------------------\n";
	for (int i = 0; i < size; i++)
	{
		for (int j = 0; j < size; j++)
		{
			cout << Revers[i][j] << " ";
		}
		cout << endl;
	}
	cout << "X\n-------------------------------\n";
	for (int i = 0; i < size; i++)
	{
		cout << X[i] << " ";
	}
	cout << endl;
}
int main()
{
	/*-------------Переменные----------------*/
	int k = 0;			//Количество итераций
	int size = 2;		//Размер матриц
	double E = 0.00000001;	//Точность

	/*--------------------Массивы-------------------*/
	double **rev;							//Обратная матрица W^(-1)
	double *f = new double[size];			//F(x)
	double **ff = new double*[size];		//Матрица Якоби W
	for (int i = 0; i < size; i++)
	{
		ff[i] = new double[size];
	}
	double *X = new double[size] { 0 };		//Х(n)
	double *x = new double[size] { 2, 1 };	//Х(n-1)
	double *mult = new double[size] { 0 };	//Умножение

	/*------------------------------------------------*/
	while (X[0] - x[0] < E || X[1] - x[1] < E)
	{
		//Приравнивание Х -> x
		if (k > 0)
		{
			for (int i = 0; i < size; i++)
			{
				x[i] = X[i];
			}
		}
		//Нахождение функции от х
		for (int i = 0; i < size; i++)
		{
			f[i] = F(i, x[0], x[1]);
		}
		//Нахождение матрицы Якоби
		ff = FF(ff, x[0], x[1]);

		//Умножение F * Rev
		rev = Reverse(ff, size);
		for (int i = 0; i < size; i++)
		{
			for (int j = 0; j < size; j++)
			{
				mult[i] = rev[i][j] * f[j] + mult[i];
			}
		}
		for (int i = 0; i < size; i++)
		{
			X[i] = x[i] - mult[i];
		}
		Output(k, size, ff, rev, X);
		k++;
	}
	return 0;
}

