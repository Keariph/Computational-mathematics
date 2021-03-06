// Integration.cpp: определяет точку входа для консольного приложения.
//

#include "stdafx.h"
#include <iostream>
#include <clocale>
#include <cmath>
using namespace std;
double f(double x);
double Trapeze(double X1, double X2, double h);
double Simpson(double X1, double X2, double h);
int main()
{
	int M;
	double h = 0.1, A, B, T = 0, S = 0;
	setlocale(LC_ALL, "rus");
	cout << "Введите интервалы и метод расчета(1 - Трапеция, 2 - Симрпсона)\t";
	cin >> A >> B >> M;
	 S+= (h/6)*(f(A) + f(B));
	for (double i = A; i < B; i += h)
	{
		if (M == 1)
		{
			T += Trapeze(i,i+h, h);
		}
		if (M == 2)
		{
			S += Simpson(i,i+h, h);
		}
	}
	if (M == 1) 
	{
		cout << "Трапеция = " << T<<endl;
	}
	if (M == 2)
	{
		cout << "Симпсон = " << S << endl;
	}
	T = 0;
	double E = 0.1, x = h;
	for (double i = A; i < B; i += x)
	{
		if ((abs((i + x) - i)) > E)
		{

			T += Simpson(i, i + x, x);
			x = x / 2;
		}
		else 
		{
			T += Simpson(i, i + x, x);
			x = h;
		}
	}
	cout << "Симпсон с двойным пересчетом = " << T << endl;
	system("pause");
    return 0;
}
double f(double x)
{
	return sqrt(x);
}
double Trapeze(double X1, double X2, double h)
{
	double L;
	L = (h)*(f(X1) + f(X2))/2;
	return L;
}
double Simpson(double X1, double X2, double h)
{
	double L;
	L = (h/6)*(2*f(X1) + f(X2) + (4* f(X2/2)));
	return L;
}