﻿using Unit = System.ValueTuple;
using static FPLibrary.F;

namespace FPLibrary.Wrappers.Console;

public interface IHasConsole<E> where E : struct {
    IO<E, IConsoleIO> ConsoleIO { get; }
}

public interface IConsoleIO {
    ConsoleColor BgColour { get; }
    
    ConsoleColor FgColour { get; }
    
    Unit Clear();

    // (int Left, int Top) GetCursorPosition();
    
    // Unit MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft, int targetTop);
    
    // Unit MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft, int targetTop, char sourceChhar, ConsoleColor sourceForeColor, ConsoleColor sourceBackColor);

    // Unit OpenStandardError();

    // Unit OpenStandardError(int bufferSize);

    // Unit OpenStandardInput();

    // Unit OpenStandardInput(int bufferSize);

    // Unit OpenStandardOutput();

    // Unit OpenStandardOutput(int bufferSize);

    Maybe<int> Read();
    
    Maybe<ConsoleKeyInfo> ReadKey();
    
    // Maybe<ConsoleKeyInfo> ReadKey(bool intercept);

    Maybe<string> ReadLine();

    // Unit ResetColour();

    // Unit SetBufferSize(int width, int height);

    // Unit SetCursorPosition(int left, int top);
    
    // Unit SetError(TextWriter newError);
    
    // Unit SetIn(TextReader newIn);
    
    // Unit SetOut(TextWriter newOut);
    
    // Unit SetWindowPosition(int left, int top);
    
    // Unit SetWindowSize(int width, int height);

    Unit Write(string value);

    Unit WriteLine(string value);

    Unit WriteLine();
}

