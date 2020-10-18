﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

namespace TP1_Math
{
    class StateTable
    {
        private Grammaire _grammaire;
        public Dictionary<string, StateTransition> _tableStructure { set; get; }

        public StateTable()
        {
            //_grammaire = grammaire;
            _tableStructure = new Dictionary<string, StateTransition>();
        }
        public void SetGrammar(Grammaire gram)
        {
            _grammaire = gram;
        }
        //CELA VA PERMETTRE DE EVENTUELLEMENT TRANSFORMER UN AUTOMATE NON-DETERMINISTE A UN DETERMINISTE
        public void CreateNDFAStateTable()
        {
            List<string> ruleList = _grammaire.Regles;
            Regex rx = new Regex("^[0-1]{1}[A-Z]{1}$");

            foreach (string r in ruleList)
            {
                int getTerminal = r.Contains("0") ? 0 : 1;
                string[] split = r.Split("->");
                string nextState = split[1];

                if (rx.IsMatch(nextState)) nextState = nextState.Substring(1);
                else
                {
                    if (nextState != "e") nextState = "SF";
                    if (!_tableStructure.ContainsKey("SF") && nextState != "e")
                    {
                        StateTransition stateTransition = new StateTransition();
                        stateTransition.IsFinalState = true;
                        _tableStructure.Add(nextState, stateTransition);
                    }
                }

                if (_tableStructure.ContainsKey(split[0]))
                {
                    if (nextState == "e") _tableStructure[split[0]].IsFinalState = true;
                    else _tableStructure[split[0]].SetNextState(getTerminal, nextState);
                }
                else
                {
                    StateTransition stateTransition = new StateTransition();
                    if (nextState == "e")
                    {
                        _tableStructure.Add(split[0], stateTransition);
                        _tableStructure[split[0]].IsFinalState = true;
                    }
                    else
                    {
                        stateTransition.SetNextState(getTerminal, nextState);
                        _tableStructure.Add(split[0], stateTransition);
                    } 
                }

            }
            //Just for debugging
            printTable();
        }

        //TODO
        public void CnvertToDFATable()
        {

        }

        public void printTable()
        {
            Console.WriteLine("State\tInput(0)\tInput(1)\tIs it a finale state?");
            foreach (var kvp in _tableStructure)
            {
                string zero = "", one = "";
                foreach (var s in kvp.Value.NextState[0])
                {
                    zero += s;
                }

                foreach (var s in kvp.Value.NextState[1])
                {
                    one += s;
                }

                bool final = kvp.Value.IsFinalState;

                Console.WriteLine($"{kvp.Key}\t{zero},\t\t{one},\t\t{final}");
            }
        }
    }
}
