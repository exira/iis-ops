﻿namespace Exira.IIS.Processor

// TODO: Would it make more sense to have 1 Railway for Exira.IIS in total?

module Railway =
    type Error =
        | UnknownEvent of string
        | DeserializeProblem of string

    type Result<'T> =
        | Success of 'T
        | Failure of Error

    let bind switchFunction = function
        | Success s -> switchFunction s
        | Failure f -> Failure f

    let bindAsync switchFunction = function
        | Success s -> switchFunction s
        | Failure f -> async { return Failure f }

    let (>>=) input switchFunction = bind switchFunction input
    let (>>=!) input switchFunction = bindAsync switchFunction input