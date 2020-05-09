using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rapadura
{

    public class Language
    {
        
        public enum LangList
        {
            select = -1, english = 0, português = 1, español = 2, français = 3, 日本の = 4
        }

        static Dictionary<LangList, Dictionary<string, string>> texts = new Dictionary<LangList, Dictionary<string, string>>
        {
             {
                LangList.select, new Dictionary<string, string>
                    {
                        { "RapaduraTooltip", "More Rapadura Assets and Tools, click here!"},
                        { "show", "Show" },
                        { "hide", "Hide" },
                        { "movimentTitle", "Movement"},
                        { "statusTitle", "Status"},
                        { "generalTitle", "General"},
                        { "platformSpritLookingRight", "Monster sprite \nfaces right"},
                        { "platformSpritLookingLeft", "Monster sprite \nfaces left"},
                        { "movimentTypes", "Movement types"},
                        { "movimentSpeed", "Movement speed"},
                        { "isGrounded", "Is grounded"},
                        { "alertNotImplementedYet", "Not Implemented yet!"},
                        { "alertGameMovimentType", "Game Movement Type is not selected in general section!"},
                        { "alertGeneralIsGlobal", "All things at this session are global to all AIs, be careful!"},
                        { "languageLabel", "Language"},
                        { "gameMovimentTypeLabel", "Game Movement Type"},
                        { "groundVerifierLabel", "Ground verifier"},
                        { "groundVerifierTooltip", "Distance between the collider and the ground to tell controller the it has landed."},
                        { "groundLayerLabel", "Ground layer"},
                        { "platformLayerLabel", "Platform layer"},
                        { "numberOfBehaviours", "Number of behaviours" },
                        { "MomentOfAction", "AI Moment of Action" },
                        { "nameOfAction", "Title of action" },
                        { "colorOfAction", "Color of action" }
                    }
            },
            {
                LangList.english, new Dictionary<string, string>
                    {
                        { "RapaduraTooltip", "More Rapadura Assets and Tools, click here!"},
                        { "show", "Show" },
                        { "hide", "Hide" },
                        { "movimentTitle", "Movement"},
                        { "statusTitle", "Status"},
                        { "generalTitle", "General"},
                        { "platformSpritLookingRight", "Monster sprite \nfaces right"},
                        { "platformSpritLookingLeft", "Monster sprite \nfaces left"},
                        { "movimentTypes", "Movement types"},
                        { "movimentSpeed", "Movement speed"},
                        { "isGrounded", "Is grounded"},
                        { "alertNotImplementedYet", "Not Implemented yet!"},
                        { "alertGameMovimentType", "Game Movement Type is not selected in general section!"},
                        { "alertGeneralIsGlobal", "All things at this session are global to all AIs, be careful!"},
                        { "languageLabel", "Language"},
                        { "gameMovimentTypeLabel", "Game Movement Type"},
                        { "groundVerifierLabel", "Ground verifier"},
                        { "groundVerifierTooltip", "Distance between the collider and the ground to tell controller the it has landed."},
                        { "groundLayerLabel", "Ground layer"},
                        { "platformLayerLabel", "Platform layer"},
                        { "numberOfBehaviours", "Number of behaviours" },
                        { "MomentOfAction", "AI Moment of Action" },
                        { "nameOfAction", "Title of action" },
                        { "colorOfAction", "Color of action" }
                    }
            },
            {
                LangList.português, new Dictionary<string, string>
                    {
                        { "RapaduraTooltip", "Mais Assets e ferramentas da Rapadura, clique aqui!"},
                        { "show", "Mostrar" },
                        { "hide", "Esconder" },
                        { "movimentTitle", "Movimento"},
                        { "statusTitle", "Estado"},
                        { "generalTitle", "Geral"},
                        { "platformSpritLookingRight", "Sprite do monstro \nestá para a direita"},
                        { "platformSpritLookingLeft", "Sprite do monstro \nestá para a esquerda"},
                        { "movimentTypes", "Tipos de movimento"},
                        { "movimentSpeed", "Velocidade do movimento"},
                        { "isGrounded", "Está no chão"},
                        { "alertNotImplementedYet", "Não implementado ainda!"},
                        { "alertGameMovimentType", "O tipo de movimento do jogo não está selecionado na seção geral!"},
                        { "alertGeneralIsGlobal", "Todas as configurações nesta sessão são globais para todos os AIs, tenha cuidado!"},
                        { "languageLabel", "Linguagem"},
                        { "gameMovimentTypeLabel", "Tipo de movimento do jogo"},
                        { "groundVerifierLabel", "Verificador de chão"},
                        { "groundVerifierTooltip", "Distância entre o colisor e o chão para dizer ao controlador que pousou."},
                        { "groundLayerLabel", "Layer do chão"},
                        { "platformLayerLabel", "Layer da plataforma"},
                        { "numberOfBehaviours", "Número de comportamentos" },
                        { "MomentOfAction", "AI Momento da Ação" },
                        { "nameOfAction", "Título da ação" },
                        { "colorOfAction", "Cor da ação" }
                    }
            },
            {
                LangList.español, new Dictionary<string, string>
                    {
                        { "RapaduraTooltip", "Más Rapadura Activos y Herramientas, haga clic aquí!"},
                        { "show", "Mostrar" },
                        { "hide", "Esconder" },
                        { "movimentTitle", "Movimiento"},
                        { "statusTitle", "Estado"},
                        { "generalTitle", "General"},
                        { "platformSpritLookingRight", "Monster sprite se \nenfrenta a la derecha"},
                        { "platformSpritLookingLeft", "Monster sprite se \nenfrenta a la izquierda"},
                        { "movimentTypes", "Tipos de movimiento"},
                        { "movimentSpeed", "Velocidad de movimiento"},
                        { "isGrounded", "Se fundamenta"},
                        { "alertNotImplementedYet", "¡Aun no implementado!"},
                        { "alertGameMovimentType", "¡El tipo de movimiento del juego no está seleccionado en la sección general!"},
                        { "alertGeneralIsGlobal", "¡Todas las cosas en esta sesión son globales para todos los AIs, ten cuidado!"},
                        { "languageLabel", "Idioma"},
                        { "gameMovimentTypeLabel", "Tipo de movimiento del juego"},
                        { "groundVerifierLabel", "Verificador de tierra"},
                        { "groundVerifierTooltip", "Distancia entre el colisionador y el suelo para decir al controlador que ha aterrizado."},
                        { "groundLayerLabel", "Capa de suelo"},
                        { "platformLayerLabel", "Capa de plataforma"},
                        { "numberOfBehaviours", "Número de comportamientos" },
                        { "MomentOfAction", "AI Momento de Acción" },
                        { "nameOfAction", "Título de la acción" },
                        { "colorOfAction", "Color de acción" }
                    }
            },
            {
                LangList.français, new Dictionary<string, string>
                    {
                        { "RapaduraTooltip", "Plus d'actifs et outils Rapadura, cliquez ici!"},
                        { "show", "Montrer" },
                        { "hide", "Cacher" },
                        { "movimentTitle", "Mouvement"},
                        { "statusTitle", "Statut"},
                        { "generalTitle", "Général"},
                        { "platformSpritLookingRight", "Monster Sprite \nfait face à droite"},
                        { "platformSpritLookingLeft", "Monster sprite \nface gauche"},
                        { "movimentTypes", "Types de mouvements"},
                        { "movimentSpeed", "Vitesse de mouvement"},
                        { "isGrounded", "Est mis à la terre"},
                        { "alertNotImplementedYet", "Pas encore mis en œuvre!"},
                        { "alertGameMovimentType", "Le type de mouvement de jeu n'est pas sélectionné dans la section générale!"},
                        { "alertGeneralIsGlobal", "Toutes les choses de cette session sont globales pour toutes les IA, faites attention!"},
                        { "languageLabel", "La langue"},
                        { "gameMovimentTypeLabel", "Type de mouvement de jeu"},
                        { "groundVerifierLabel", "Vérificateur de sol"},
                        { "groundVerifierTooltip", "Distance entre le collisionneur et le sol pour indiquer au contrôleur qu'il a atterri."},
                        { "groundLayerLabel", "Couche de sol"},
                        { "platformLayerLabel", "Couche de plate-forme"},
                        { "numberOfBehaviours", "Nombre de comportements" },
                        { "MomentOfAction", "AI Moment d'action" },
                        { "nameOfAction", "Titre de l'action" },
                        { "colorOfAction", "Couleur d'action" }
                    }
            },
            {
                LangList.日本の, new Dictionary<string, string>
                    {
                        { "RapaduraTooltip", "ラパドゥーラ資産とツールの詳細はこちらをクリック!"},
                        { "show", "ショー" },
                        { "hide", "隠す" },
                        { "movimentTitle", "移動"},
                        { "statusTitle", "状態"},
                        { "generalTitle", "一般"},
                        { "platformSpritLookingRight", "モンスタースプライ\nトが右を向いている"},
                        { "platformSpritLookingLeft", "モンスタースプライ\nトが左を向いている"},
                        { "movimentTypes", "移動タイプ"},
                        { "movimentSpeed", "移動速度"},
                        { "isGrounded", "接地されている"},
                        { "alertNotImplementedYet", "まだ実装されていません！"},
                        { "alertGameMovimentType", "一般的なセクションでゲームの移動タイプが選択されていません！"},
                        { "alertGeneralIsGlobal", "このセッションのすべてのものはすべてのAIにとってグローバルなものですので、注意してください！"},
                        { "languageLabel", "言語"},
                        { "gameMovimentTypeLabel", "ゲームの移動タイプ"},
                        { "groundVerifierLabel", "地上検証者"},
                        { "groundVerifierTooltip", "コライダーと地面との間の距離で、コントローラーに着陸したことを伝えます。"},
                        { "groundLayerLabel", "地層"},
                        { "platformLayerLabel", "プラットフォーム層"},
                        { "numberOfBehaviours", "行動の数" },
                        { "MomentOfAction", "AIの行動の瞬間" },
                        { "nameOfAction", "行動のタイトル" },
                        { "colorOfAction", "行動の色" }
                    }
            }
        };


        public static string GetRapaduraTooltip(Language.LangList selectedLanguage)
        {
            return texts[selectedLanguage]["RapaduraTooltip"];
        }

        public static string show(Language.LangList selectedLanguage)
        {
            return texts[selectedLanguage]["show"];
        }

        public static string hide(Language.LangList selectedLanguage)
        {
            return texts[selectedLanguage]["hide"];
        }

        public static string movimentTitle(Language.LangList selectedLanguage)
        {
            return texts[selectedLanguage]["movimentTitle"];
        }

        public static string statusTitle(Language.LangList selectedLanguage)
        {
            return texts[selectedLanguage]["statusTitle"];
        }

        public static string generalTitle(Language.LangList selectedLanguage)
        {
            return texts[selectedLanguage]["generalTitle"];
        }

        public static string platformSpritLookingRight(Language.LangList selectedLanguage)
        {
            return texts[selectedLanguage]["platformSpritLookingRight"];
        }

        public static string platformSpritLookingLeft(Language.LangList selectedLanguage)
        {
            return texts[selectedLanguage]["platformSpritLookingLeft"];
        }

        public static string movimentTypes(Language.LangList selectedLanguage)
        {
            return texts[selectedLanguage]["movimentTypes"];
        }

        public static string movimentSpeed(Language.LangList selectedLanguage)
        {
            return texts[selectedLanguage]["movimentSpeed"];
        }

        public static string isGrounded(Language.LangList selectedLanguage)
        {
            return texts[selectedLanguage]["isGrounded"];
        }

        public static string alertNotImplementedYet(Language.LangList selectedLanguage)
        {
            return texts[selectedLanguage]["alertNotImplementedYet"];
        }

        public static string alertGameMovimentType(Language.LangList selectedLanguage)
        {
            return texts[selectedLanguage]["alertGameMovimentType"];
        }


        public static string alertGeneralIsGlobal(Language.LangList selectedLanguage)
        {
            return texts[selectedLanguage]["alertGeneralIsGlobal"];
        }

        public static string languageLabel(Language.LangList selectedLanguage)
        {
            return texts[selectedLanguage]["languageLabel"];
        }

        public static string gameMovimentTypeLabel(Language.LangList selectedLanguage)
        {
            return texts[selectedLanguage]["gameMovimentTypeLabel"];
        }

        public static string groundVerifierLabel(Language.LangList selectedLanguage)
        {
            return texts[selectedLanguage]["groundVerifierLabel"];
        }

        public static string groundVerifierTooltip(Language.LangList selectedLanguage)
        {
            return texts[selectedLanguage]["groundVerifierTooltip"];
        }

        public static string groundLayerLabel(Language.LangList selectedLanguage)
        {
            return texts[selectedLanguage]["groundLayerLabel"];
        }

        public static string platformLayerLabel(Language.LangList selectedLanguage)
        {
            return texts[selectedLanguage]["platformLayerLabel"];
        }

        public static string numberOfBehaviours(Language.LangList selectedLanguage)
        {
            return texts[selectedLanguage]["numberOfBehaviours"];
        }

        public static string MomentOfAction(Language.LangList selectedLanguage)
        {
            return texts[selectedLanguage]["MomentOfAction"];
        }

        public static string nameOfAction(Language.LangList selectedLanguage)
        {
            return texts[selectedLanguage]["nameOfAction"];
        }

        public static string colorOfAction(Language.LangList selectedLanguage)
        {
            return texts[selectedLanguage]["colorOfAction"];
        }
    }

}