[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Ionide.VSCode.FSharp

open System
open System.Text.RegularExpressions
open Fable.Core
open Fable.Core.JsInterop
open Fable.Import.vscode
open Ionide.VSCode.Helpers
open Ionide.VSCode.FSharp

let activate(disposables: Disposable[]) =
    let df = createEmpty<DocumentFilter>
    df.language <- Some "fsharp"
    let df' : DocumentSelector = df |> U3.Case2

    LanguageService.start()
    |> Promise.success (fun () ->
        Linter.activate disposables
        |> Promise.bind(fun _ -> Project.activate ())
        |> ignore

        Tooltip.activate df' disposables
        Autocomplete.activate df' disposables
        ParameterHints.activate df' disposables
        Definition.activate df' disposables
        Reference.activate df' disposables
        Symbols.activate df' disposables
        Highlights.activate df' disposables
        Rename.activate df' disposables
        WorkspaceSymbols.activate df' disposables
        QuickInfo.activate disposables
    )
    |> Promise.fail (fun x ->
        if Process.isMono () then
            "Failed to start language services. Please check if mono is in PATH"
        else
            "Failed to start language services. Please check if Microsoft Build Tools 2013 are installed"
        |> Fable.Import.vscode.window.showErrorMessage
        |> ignore
    ) |> ignore
    

    Forge.activate disposables
    Fsi.activate disposables
    WebPreview.activate disposables


    ()

let deactivate(disposables: Disposable[]) =
    LanguageService.stop ()

