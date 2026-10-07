using System;
using System.Collections.Generic;

namespace CupomCompleto
{
	class Program
	{
		// ---------- funcoes auxiliares ----------

		// le um numero decimal e repete a pergunta ate o usuario digitar algo valido
		static double LerDouble(string mensagem, double minimo, double maximo)
		{
			double valor;
			while (true)
			{
				Console.Write(mensagem);
				// TryParse devolve false se o texto nao for numero, sem dar erro no programa
				if (double.TryParse(Console.ReadLine(), out valor) && valor >= minimo && valor <= maximo)
					return valor;
				Console.WriteLine("  Valor invalido. Tente de novo (use virgula nos decimais).");
			}
		}

		// mesma ideia, mas para numeros inteiros
		static int LerInt(string mensagem, int minimo, int maximo)
		{
			int valor;
			while (true)
			{
				Console.Write(mensagem);
				if (int.TryParse(Console.ReadLine(), out valor) && valor >= minimo && valor <= maximo)
					return valor;
				Console.WriteLine("  Valor invalido. Digite um inteiro entre " + minimo + " e " + maximo + ".");
			}
		}

		// pergunta de sim/nao: devolve true se a resposta for "s"
		static bool LerSimNao(string mensagem)
		{
			while (true)
			{
				Console.Write(mensagem);
				string resposta = Console.ReadLine().Trim().ToLower();
				if (resposta == "s") return true;
				if (resposta == "n") return false;
				Console.WriteLine("  Responda apenas com s ou n.");
			}
		}

		// valor do desconto = percentual aplicado sobre o valor
		static double CalcularDesconto(double valor, double percentual)
		{
			return valor * percentual / 100;
		}

		// valor do imposto = taxa aplicada sobre o valor
		static double CalcularImposto(double valor, double taxa)
		{
			return valor * taxa / 100;
		}

		// deixa o valor no formato de dinheiro
		static string Moeda(double valor)
		{
			return "R$ " + valor.ToString("F2");
		}

		// ---------- programa principal ----------

		public static void Main(string[] args)
		{
			// listas guardam os dados de todos os produtos do cupom
			List<string> nomes = new List<string>();
			List<double> precos = new List<double>();
			List<double> descontos = new List<double>();   // percentual de cada produto
			List<double> precosFinais = new List<double>(); // preco ja com o desconto

			Console.WriteLine("=== SIMULADOR DE CUPOM FISCAL ===");

			// ---- varios produtos: repete enquanto o usuario quiser adicionar ----
			bool maisProdutos = true;
			while (maisProdutos)
			{
				Console.WriteLine("\n--- Produto " + (nomes.Count + 1) + " ---");

				string nome = "";
				while (nome.Trim() == "")
				{
					Console.Write("Nome do produto: ");
					nome = Console.ReadLine();
				}

				double preco = LerDouble("Preco (R$): ", 0.01, 1000000);
				double descPercentual = LerDouble("Desconto (0 a 100%): ", 0, 100);

				// calculos da parte 1
				double valorDesconto = CalcularDesconto(preco, descPercentual);
				double precoFinal = preco - valorDesconto;

				nomes.Add(nome.Trim());
				precos.Add(preco);
				descontos.Add(descPercentual);
				precosFinais.Add(precoFinal);

				maisProdutos = LerSimNao("Adicionar outro produto? (s/n): ");
			}

			// ---- soma dos produtos ----
			double totalOriginal = 0;
			double subtotal = 0;
			for (int i = 0; i < nomes.Count; i++)
			{
				totalOriginal += precos[i];
				subtotal += precosFinais[i];
			}

			// ---- desconto VIP (5% extra) ----
			Console.WriteLine();
			bool vip = LerSimNao("Cliente VIP? (s/n): ");
			double descontoVip = vip ? CalcularDesconto(subtotal, 5) : 0;

			// ---- cupom promocional ----
			Console.Write("Codigo promocional (Enter para pular): ");
			string codigo = Console.ReadLine().Trim().ToUpper();
			double descontoCupom = 0;
			if (codigo == "PROMO10")
			{
				// o cupom incide sobre o valor que sobrou depois do desconto VIP
				descontoCupom = CalcularDesconto(subtotal - descontoVip, 10);
				Console.WriteLine("  Cupom PROMO10 aplicado: 10% de desconto!");
			}
			else if (codigo != "")
			{
				Console.WriteLine("  Cupom invalido, nenhum desconto aplicado.");
			}

			// ---- imposto ----
			double baseImposto = subtotal - descontoVip - descontoCupom;
			double taxaImposto = LerDouble("Taxa de imposto (%): ", 0, 100);
			double imposto = CalcularImposto(baseImposto, taxaImposto);
			double totalFinal = baseImposto + imposto;

			// ---- forma de pagamento ----
			int forma = LerInt("Pagamento (1 = a vista, 2 = parcelado): ", 1, 2);
			int parcelas = 1;
			if (forma == 2)
				parcelas = LerInt("Quantas parcelas (2 a 12)? ", 2, 12);
			double valorParcela = totalFinal / parcelas;

			// ---- impressao do cupom ----
			string linha = new string('=', 54);
			string traco = new string('-', 54);

			Console.WriteLine();
			Console.WriteLine(linha);
			Console.WriteLine("                     CUPOM FISCAL");
			Console.WriteLine(linha);
			Console.WriteLine("Data/Hora: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
			Console.WriteLine(traco);
			Console.WriteLine("PRODUTO".PadRight(20) + "PRECO".PadLeft(11) + "DESC.".PadLeft(8) + "FINAL".PadLeft(13));
			Console.WriteLine(traco);

			for (int i = 0; i < nomes.Count; i++)
			{
				// corta nomes muito grandes pra nao desalinhar a tabela
				string nomeCurto = nomes[i].Length > 18 ? nomes[i].Substring(0, 18) : nomes[i];

				Console.WriteLine(nomeCurto.PadRight(20)
					+ precos[i].ToString("F2").PadLeft(11)
					+ (descontos[i].ToString("F0") + "%").PadLeft(8)
					+ precosFinais[i].ToString("F2").PadLeft(13));
			}

			Console.WriteLine(traco);
			Console.WriteLine("Total sem desconto.......: " + Moeda(totalOriginal));
			Console.WriteLine("Desconto nos produtos....: " + Moeda(totalOriginal - subtotal));
			Console.WriteLine("Subtotal.................: " + Moeda(subtotal));
			if (vip)
				Console.WriteLine("Desconto VIP (5%)........: " + Moeda(descontoVip));
			if (descontoCupom > 0)
				Console.WriteLine("Cupom PROMO10 (10%)......: " + Moeda(descontoCupom));
			Console.WriteLine("Imposto (" + taxaImposto.ToString("F2") + "%)........: " + Moeda(imposto));
			Console.WriteLine(traco);
			Console.WriteLine("TOTAL A PAGAR............: " + Moeda(totalFinal));

			if (forma == 1)
				Console.WriteLine("Pagamento................: A vista");
			else
				Console.WriteLine("Pagamento................: " + parcelas + "x de " + Moeda(valorParcela));

			Console.WriteLine(linha);
			Console.WriteLine("          Obrigado pela preferencia!");
			Console.WriteLine(linha);

			Console.Write("\nPressione qualquer tecla para sair...");
			Console.ReadKey(true);
		}
	}
}
