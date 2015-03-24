﻿namespace Exira.IIS.Domain

module Railway =
    type Error =
        | UnknownCommand of string
        | InvalidState of string
        | InvalidStateTransition of string

    type Result<'T> =
        | Success of 'T
        | Failure of Error

    let bind switchFunction = function
        | Success s -> switchFunction s
        | Failure f -> Failure f

    let (>>=) input switchFunction = bind switchFunction input