using System;

namespace CupomBase
{
	class Program
	{
		public static void Main(string[] args)
		{
			// 1) entrada de dados (preco e desconto com virgula, ex: 19,90)
			Console.Write("Nome do produto: ");
			string nome = Console.ReadLine();
			Console.Write("Preco do produto (R$): ");
			double preco = double.Parse(Console.ReadLine());
			Console.Write("Desconto (%): ");
			double descontoPercentual = double.Parse(Console.ReadLine());

			// 2) calculos: valor do desconto e preco final
			double valorDesconto = preco * descontoPercentual / 100;
			double precoFinal = preco - valorDesconto;

			// 3) exibicao no formato de cupom
			Console.WriteLine();
			Console.WriteLine("==============================");
			Console.WriteLine("         CUPOM FISCAL         ");
			Console.WriteLine("==============================");
			Console.WriteLine("Produto.........: " + nome);
			Console.WriteLine("Preco original..: R$ " + preco.ToString("F2"));
			Console.WriteLine("Desconto........: " + descontoPercentual.ToString("F2") + "%");
			Console.WriteLine("Valor desconto..: R$ " + valorDesconto.ToString("F2"));
			Console.WriteLine("------------------------------");
			Console.WriteLine("PRECO FINAL.....: R$ " + precoFinal.ToString("F2"));
			Console.WriteLine("==============================");

			Console.Write("\nPressione qualquer tecla para sair...");
			Console.ReadKey(true);
		}
	}
}
