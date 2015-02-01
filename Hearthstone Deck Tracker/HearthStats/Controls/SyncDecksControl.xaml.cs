﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Hearthstone_Deck_Tracker.Hearthstone;

namespace Hearthstone_Deck_Tracker.HearthStats.Controls
{
	/// <summary>
	/// Interaction logic for SyncDecksControl.xaml
	/// </summary>
	public partial class SyncDecksControl : UserControl
	{
		public SyncDecksControl()
		{
			InitializeComponent();
		}

		public void LoadLocalDecks(IEnumerable<Deck> decks)
		{
			ListViewLocalDecksNoSync.Items.Clear();
			ListViewLocalDecksSync.Items.Clear();
			foreach(var deck in decks.OrderBy(x => x.Name))
			{
				if(!deck.SyncWithHearthStats.HasValue)
					deck.SyncWithHearthStats = true;
				if(deck.SyncWithHearthStats.Value)
				{
					ListViewLocalDecksSync.Items.Add(deck);
				}
				else
					ListViewLocalDecksNoSync.Items.Add(deck);
			}
			foreach(var deck in new[] { new Deck() { Name = "test deck 1" } , new Deck() { Name = "test deck 2" } })
				ListViewHearthStats.Items.Add(deck);
		}

		private void DeckDoNotSync_OnChecked(object sender, RoutedEventArgs e)
		{
			var cb = sender as CheckBox;
			if(cb == null)
				return;
			var deck = cb.DataContext as Deck;
			if(deck == null)
				return;
			ListViewLocalDecksNoSync.Items.Remove(deck);
			ListViewLocalDecksSync.Items.Add(deck);
            ListViewLocalDecksSync.Items.SortDescriptions.Clear();
			ListViewLocalDecksSync.Items.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
			ListViewLocalDecksSync.SelectedItem = deck;
		}

		private void DeckDoSync_OnChecked(object sender, RoutedEventArgs e)
		{
			var cb = sender as CheckBox;
			if(cb == null)
				return;
			var deck = cb.DataContext as Deck;
			if(deck == null)
				return;
			ListViewLocalDecksSync.Items.Remove(deck);
			ListViewLocalDecksNoSync.Items.Add(deck);
			ListViewLocalDecksNoSync.Items.SortDescriptions.Clear();
			ListViewLocalDecksNoSync.Items.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
			ListViewLocalDecksNoSync.SelectedItem = deck;
		}

		private void BtnSyncAll_Click(object sender, RoutedEventArgs e)
		{
			foreach(var deck in ListViewLocalDecksNoSync.Items.Cast<Deck>().ToList())
			{
				ListViewLocalDecksNoSync.Items.Remove(deck);
				deck.SyncWithHearthStats = true;
				ListViewLocalDecksSync.Items.Add(deck);
			}
			ListViewLocalDecksSync.Items.SortDescriptions.Clear();
			ListViewLocalDecksSync.Items.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
		}

		private void BtnSyncNone_Click(object sender, RoutedEventArgs e)
		{
			foreach(var deck in ListViewLocalDecksSync.Items.Cast<Deck>().ToList())
			{
				ListViewLocalDecksSync.Items.Remove(deck);
				deck.SyncWithHearthStats = false;
				ListViewLocalDecksNoSync.Items.Add(deck);
			}
			ListViewLocalDecksNoSync.Items.SortDescriptions.Clear();
			ListViewLocalDecksNoSync.Items.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
		}
	}
}
