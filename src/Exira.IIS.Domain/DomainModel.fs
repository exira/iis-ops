﻿namespace Exira.IIS.Domain

module DomainModel =
    open Exira.ErrorHandling

    open DomainTypes

    type ServerInfo = {
        ServerId: ServerId.T
        Name: string
        Dns: Hostname.T
        Description: string
    }

    type Server =
    | Init
    | Created of ServerInfo
    | Deleted

    type Error =
    // Validation errors
    | ServerIdIsRequired
    | ServerIdMustNotBeEmpty
    | HostnameIsRequired
    | HostnameMustBeValid

    // State errors
    | ServerDoesNotExist
    | ServerAlreadyCreated
    | ServerAlreadyDeleted

    // Internal errors
    | InvalidState of string
    | InvalidStateTransition of string

    let private constructionSuccess value =
        succeed value

    let private constructionFailure value =
        fail [value]

    let private construct ctor value =
        value
        |> ctor constructionSuccess constructionFailure

    let createServerId serverId =
        let map = function
        | GuidError.Missing -> ServerIdIsRequired
        | MustNotBeEmpty _ -> ServerIdMustNotBeEmpty

        serverId
        |> construct ServerId.createWithCont
        |> mapMessages map

    let createHostname hostname =
        let map = function
        | UriError.Missing -> HostnameIsRequired
        | Unknown -> HostnameMustBeValid

        hostname
        |> construct Hostname.createWithCont
        |> mapMessages map
