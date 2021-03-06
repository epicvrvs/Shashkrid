﻿using System;
using System.Collections.Generic;
using System.Net.Sockets;

using Nil;

using Shashkrid;

namespace Client
{
	class Client
	{
		ClientConfiguration Configuration;

		public Client(ClientConfiguration configuration)
		{
			Configuration = configuration;
		}

		public void Run()
		{
			TcpClient client = new TcpClient();
			client.Connect(Configuration.Server);
			while (true)
			{
				Console.Write("> ");
				string line = Console.ReadLine();
				List<string> tokens = Nil.String.Tokenise(line, " ");
				if (tokens.Count == 0)
					continue;
				string command = tokens[0];
				List<string> arguments = tokens.GetRange(1, tokens.Count - 1);
				throw new NotImplementedException();
			}
		}

		void Write(string text, ConsoleColor colour)
		{
			Console.ForegroundColor = colour;
			Console.Write(text);
		}

		void RenderBoard()
		{
			const ConsoleColor labelColour = ConsoleColor.White;
			const ConsoleColor boardColour = ConsoleColor.DarkGray;
			const ConsoleColor blackColour = ConsoleColor.Red;
			const ConsoleColor whiteColour = ConsoleColor.Yellow;

			Dictionary<PieceTypeIdentifier, string> pieceSymbols = new Dictionary<PieceTypeIdentifier, string>()
			{
				{PieceTypeIdentifier.Pawn, "P"},
				{PieceTypeIdentifier.Martyr, "M"},
				{PieceTypeIdentifier.Guardian, "G"},
				{PieceTypeIdentifier.Lance, "C"},
				{PieceTypeIdentifier.Serpent, "S"},
			};

			const string emptyHex = "o";
			const string genericFiller = "--";
			const string verticalFillerLeft = "- ";
			const string verticalFillerRight = " -";

			string space = "";
			for (int i = 0; i < GameConstants.GridSizeX * 2; i++)
				space += " ";
			string xLabels = GetXLabels();
			Write(space + xLabels + "\n", labelColour);
			for (int row = GameConstants.GridSizeY; row >= 1; row--)
			{
				string leftLabel = row.ToString();
				while (leftLabel.Length < row * 2)
					leftLabel = " " + leftLabel;
				leftLabel += " ";
				string rightLabel = " " + row.ToString();
				Write(leftLabel, labelColour);
				for (int column = 1; column <= GameConstants.GridSizeX; column++)
				{
					Position position = new Position(row, column);
					Hex hex = Messenger.Game.GetHex(position);
					if (hex.Piece == null)
						Write(emptyHex, boardColour);
					else
					{
						Piece piece = hex.Piece;
						ConsoleColor colour = piece.Owner.Colour == PlayerColour.Black ? blackColour : whiteColour;
						string symbol = pieceSymbols[piece.Type.Identifier];
						Write(symbol, colour);
					}
					if (column < GameConstants.GridSizeX)
					{
						if (column == GameConstants.GridSizeX / 2)
							Write(verticalFillerLeft, boardColour);
						else if (column == GameConstants.GridSizeX / 2 + 1)
							Write(verticalFillerRight, boardColour);
						else
							Write(genericFiller, boardColour);
					}
				}
				Write(rightLabel + "\n", labelColour);
			}
			Write("  " + xLabels + "\n", labelColour);
			Console.ForegroundColor = ConsoleColor.Gray;
		}

		string GetXLabels()
		{
			string output = "";
			bool first = true;
			for (int i = 0; i < GameConstants.GridSizeX * 2; i++)
			{
				if (first)
					first = false;
				else
					output += " ";
				char label = (char)((int)'a' + i);
				output += label;
			}
			return output;
		}
	}
}
