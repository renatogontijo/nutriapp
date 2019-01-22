using Model;
using View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Presenter
{
    public class GerarCombinacaoAlimentarPresenter : IPresenter
    {
        // Dados em memória
        private static IDictionary<string, IModel> m_dataSet = new Dictionary<string, IModel>();
        public static int SelectedItemValue = -1;
        private const string CO_NAME = "Combinações alimentares";

        public string Name { get { return CO_NAME; } }
        public IPresenter ParentPresenter { get; set; }
        public IModel Model { get; set; }
        public IView View { get; set; }

        public void SetParentPresenter(IPresenter presenter)
        {
            ParentPresenter = presenter;
        }

        public void SetView(IView view, IModel model)
        {
            view.SetPresenter(this);
            view.SetModel(model);
            View = view;
            Model = model;
        }


        public bool Save(out string msgErr)
        {
            msgErr = null;
            return false;
        }

        public void SelectedItem(ConsoleView consoleView, int position)
        {

        }

        private bool ValidateModel(out string msgErr)
        {
            msgErr = null;
            CombinacaoAlimentar combinacao = (CombinacaoAlimentar)Model;

            if (!Regex.IsMatch(combinacao.Calorias, "\\d{1,4}"))
                msgErr += "Campo Calorias deve ser um valor numérico de no máximo 4 dígitos";

            return (string.IsNullOrEmpty(msgErr));
        }

        public void Search(ConsoleView consoleView)
        {
            string msg = null;

            if (!ValidateModel(out msg))
                consoleView.WriteMessageOnFooter(msg);
            else
            {
                LoadDataSet();
                List<Alimento> food = CreateCombination();
                ShowCombination(consoleView, food);
            }
        }

        private void CreateActions(ConsoleView consoleView)
        {
            consoleView.StartAction();

            // Voltar
            consoleView.CreateAction(ConsoleKey.F7, ParentPresenter, "Voltar");
        }

        private void LoadDataSet()
        {
            CombinacaoAlimentar combinacao = (CombinacaoAlimentar)Model;
            combinacao.Alimentos.Add(new Alimento() { NomeAlimento = "Maça", Calorias = "2", GrupoAlimentar = "1" });
            combinacao.Alimentos.Add(new Alimento() { NomeAlimento = "Melão", Calorias = "20", GrupoAlimentar = "2" });
            combinacao.Alimentos.Add(new Alimento() { NomeAlimento = "Jaca", Calorias = "200", GrupoAlimentar = "3" });
            combinacao.Alimentos.Add(new Alimento() { NomeAlimento = "Arroz", Calorias = "1000", GrupoAlimentar = "2" });
            combinacao.Alimentos.Add(new Alimento() { NomeAlimento = "Feijão", Calorias = "1500", GrupoAlimentar = "1" });
            combinacao.Alimentos.Add(new Alimento() { NomeAlimento = "Macarrão", Calorias = "3000", GrupoAlimentar = "3" });
            combinacao.Alimentos.Add(new Alimento() { NomeAlimento = "Abóbora", Calorias = "110", GrupoAlimentar = "3" });
            combinacao.Alimentos.Add(new Alimento() { NomeAlimento = "Carnes", Calorias = "250", GrupoAlimentar = "1" });
        }

        private List<Alimento> CreateCombination()
        {
            CombinacaoAlimentar combinacao = (CombinacaoAlimentar)Model;
            List<Alimento> novaCombinacao = new List<Alimento>();
            
            int caloriasTotais = Convert.ToInt32(combinacao.Calorias);
            int calorias = 0;
            int acum = 0;

            List<Alimento> grupo1 = combinacao.Alimentos.Where(c => c.GrupoAlimentar.Equals("1")).ToList();
            List<Alimento> grupo2 = combinacao.Alimentos.Where(c => c.GrupoAlimentar.Equals("2")).ToList();
            List<Alimento> grupo3 = combinacao.Alimentos.Where(c => c.GrupoAlimentar.Equals("3")).ToList();

            int grupo = 0;
            int grupo1Pos = 0;
            int grupo2Pos = 0;
            int grupo3Pos = 0;
            Alimento alimento;

            do 
            {
                alimento = null;

                switch(grupo)
                {
                    case 0:
                        if (grupo1Pos < grupo1.Count)
                            alimento = grupo1[grupo1Pos++];
                        break;
                    case 1:
                        if (grupo2Pos < grupo2.Count)
                            alimento = grupo2[grupo2Pos++];
                        break;
                    case 2:
                        if (grupo3Pos < grupo3.Count)
                            alimento = grupo3[grupo3Pos++];
                        break;
                }

                grupo = (++grupo % 3);

                calorias = Convert.ToInt32(alimento.Calorias);
                acum += calorias;

                if (acum < caloriasTotais)
                    novaCombinacao.Add(alimento);

            } while(acum < caloriasTotais);
            

            // foreach(Alimento alimento in combinacao.Alimentos)
            // {
            //     int cal = Convert.ToInt32(alimento.Calorias);
            //     acum += cal;
            //     if (acum > caloriasTotais)
            //         break;
            //     else
            //         novaCombinacao.Add(alimento);
            // }

            return novaCombinacao;
        }

        public void ShowCombination(ConsoleView consoleView, List<Alimento> food)
        {
            ((CombinacaoAlimentar)Model).CombinacoesAlimentares = food;

            IView view = new MostrarCombinacaoAlimentarView();
            
            view.SetModel(Model);
            view.SetPresenter(this);
            view.Show(consoleView, Model);
        }

        public void Show(ConsoleView consoleView)
        {
            CreateActions(consoleView);

            View.Show(consoleView, Model);
        }
    }
}