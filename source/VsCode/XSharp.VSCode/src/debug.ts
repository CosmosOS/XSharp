/*---------------------------------------------------------
 * This is based on the example given by Microsoft
 *--------------------------------------------------------*/

import {
	Logger, logger,
	LoggingDebugSession,
	InitializedEvent, TerminatedEvent, StoppedEvent, BreakpointEvent, OutputEvent,
	ProgressStartEvent, ProgressUpdateEvent, ProgressEndEvent,
	Thread, StackFrame, Scope, Source, Handles, Breakpoint
} from 'vscode-debugadapter';
import { DebugProtocol } from 'vscode-debugprotocol';
import { basename } from 'path';
import { XSharpRuntime } from './runtime';

function timeout(ms: number) {
	return new Promise(resolve => setTimeout(resolve, ms));
}

/**
 * This interface describes the mock-debug specific launch attributes
 * (which are not part of the Debug Adapter Protocol).
 * The schema for these attributes lives in the package.json of the mock-debug extension.
 * The interface should always match this schema.
 */
interface ILaunchRequestArguments extends DebugProtocol.LaunchRequestArguments {
	/** An absolute path to the "program" to debug. */
	program: string;
	/** Automatically stop target after launch. If not specified, target does not stop. */
	stopOnEntry?: boolean;
	/** enable logging the Debug Adapter Protocol */
	trace?: boolean;
	/** run without debugging */
	noDebug?: boolean;
}

export class MockDebugSession extends LoggingDebugSession {

	// we don't support multiple threads, so we can use a hardcoded ID for the default thread
	private static threadID = 1;

	private _reportProgress = false;
	private _progressId = 10000;
	private _cancelledProgressId: string | undefined = undefined;
	private false = true;
	private _runtime: XSharpRuntime;

	/**
	 * Creates a new debug adapter that is used for one debug session.
	 * We configure the default implementation of a debug adapter here.
	 */
	public constructor() {
		super();

		this._runtime = new XSharpRuntime();
		
		// this debugger uses zero-based lines and columns
		this.setDebuggerLinesStartAt1(false);
		this.setDebuggerColumnsStartAt1(false);
	}

	/**
	 * The 'initialize' request is the first request called by the frontend
	 * to interrogate the features the debug adapter provides.
	 */
	protected initializeRequest(response: DebugProtocol.InitializeResponse, args: DebugProtocol.InitializeRequestArguments): void {

		if (args.supportsProgressReporting) {
			this._reportProgress = true;
		}

		// build and return the capabilities of this debug adapter:
		response.body = response.body || {};

		// the adapter implements the configurationDoneRequest.
		response.body.supportsConfigurationDoneRequest = true;

		// make VS Code to use 'evaluate' when hovering over source
		response.body.supportsEvaluateForHovers = false;

		// make VS Code to show a 'step back' button
		response.body.supportsStepBack = false;

		// make VS Code to support data breakpoints
		response.body.supportsDataBreakpoints = false;

		// make VS Code to support completion in REPL
		response.body.supportsCompletionsRequest = false;

		// make VS Code to send cancelRequests
		response.body.supportsCancelRequest = false;

		// make VS Code send the breakpointLocations request
		response.body.supportsBreakpointLocationsRequest = false;

		// make VS Code provide "Step in Target" functionality
		response.body.supportsStepInTargetsRequest = false;

		this.sendResponse(response);

		// since this debug adapter can accept configuration requests like 'setBreakpoint' at any time,
		// we request them early by sending an 'initializeRequest' to the frontend.
		// The frontend will end the configuration sequence by calling 'configurationDone' request.
		this.sendEvent(new InitializedEvent());
	}

	protected async launchRequest(response: DebugProtocol.LaunchResponse, args: ILaunchRequestArguments) {

		// make sure to 'Stop' the buffered logging if 'trace' is not set
		logger.setup(args.trace ? Logger.LogLevel.Verbose : Logger.LogLevel.Stop, false);

		// start the program in the runtime
		this._runtime.start(args.program, !!args.stopOnEntry, !!args.noDebug);

		this.sendResponse(response);
	}

	protected threadsRequest(response: DebugProtocol.ThreadsResponse): void {

		// runtime supports no threads so just return a default thread.
		response.body = {
			threads: [
				new Thread(MockDebugSession.threadID, "thread 1")
			]
		};
		this.sendResponse(response);
	}
	//---- helpers

	private createSource(filePath: string): Source {
		return new Source(basename(filePath), this.convertDebuggerPathToClient(filePath), undefined, undefined, 'mock-adapter-data');
	}
}
