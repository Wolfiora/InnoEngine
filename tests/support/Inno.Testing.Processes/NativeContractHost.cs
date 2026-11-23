using System;
using System.Collections.Generic;

namespace Inno.Testing.Processes;

/// <summary>
/// Routes the declared native contracts of one test assembly to a separate main-thread host and executes them there.
/// </summary>
public sealed class NativeContractHost
{
    private readonly string m_assembly;
    private readonly IReadOnlyDictionary<string, Action> m_contracts;
    private bool m_executingContract;

    /// <summary>
    /// Creates the host of one executable test assembly.
    /// </summary>
    /// <param name="assembly">
    /// The absolute path of the executable test assembly that declares the contracts.
    /// </param>
    /// <param name="contracts">
    /// The contracts the assembly can execute, keyed by their exact identifier.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The assembly path is empty.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// The contracts are missing.
    /// </exception>
    public NativeContractHost(
        string assembly,
        IReadOnlyDictionary<string, Action> contracts
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(assembly);
        ArgumentNullException.ThrowIfNull(contracts);
        m_assembly = assembly;
        m_contracts = contracts;
    }

    /// <summary>
    /// Runs a contract in a separate host when the current process cannot execute it on its main thread.
    /// </summary>
    /// <param name="contract">
    /// The exact identifier of a declared contract.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the contract completed in a separate host; <see langword="false"/> when the caller
    /// must execute it in the current process.
    /// </returns>
    /// <exception cref="TimeoutException">
    /// The separate host exceeded its execution budget.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The separate host failed, could not start, or did not drain its redirected output.
    /// </exception>
    public bool RunIfRequired(string contract)
    {
        if (m_executingContract || !OperatingSystem.IsMacOS()
                                   && Environment.GetEnvironmentVariable("INNO_TEST_MAIN_THREAD_HOST") != "1"
        ) {
            return false;
        }
        NativeProcessTestHost.Run(m_assembly, contract);
        return true;
    }

    /// <summary>
    /// Executes the single contract named by the process arguments on the calling thread.
    /// </summary>
    /// <param name="arguments">
    /// The process arguments; exactly one declared contract identifier.
    /// </param>
    /// <returns>
    /// Zero when the contract completed; one when it failed or the arguments name no declared contract.
    /// </returns>
    public int Execute(string[] arguments)
    {
        try
        {
            if (arguments.Length != 1 || !m_contracts.TryGetValue(arguments[0], out Action? contract))
            {
                throw new ArgumentException("The native test host requires one declared contract.");
            }
            // Cocoa requires the process main thread, rather than an xUnit worker or a new managed thread.
            m_executingContract = true;
            Console.Error.WriteLine($"Native contract started: {arguments[0]}; thread={Environment.CurrentManagedThreadId}.");
            contract();
            Console.Error.WriteLine($"Native contract completed: {arguments[0]}.");
            return 0;
        }
        catch (Exception failure)
        {
            Console.Error.WriteLine(failure);
            return 1;
        }
    }
}
