 /*
  * PortAudioSharp - PortAudio bindings for .NET
  * Copyright 2006, Riccardo Gerosa, and individual contributors as indicated
  * by the @authors tag. See the copyright.txt in the distribution for a
  * full listing of individual contributors.
  *
  * This is free software; you can redistribute it and/or modify it
  * under the terms of the GNU Lesser General Public License as
  * published by the Free Software Foundation; either version 2.1 of
  * the License, or (at your option) any later version.
  *
  * This software is distributed in the hope that it will be useful,
  * but WITHOUT ANY WARRANTY; without even the implied warranty of
  * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
  * Lesser General Public License for more details.
  *
  * You should have received a copy of the GNU Lesser General Public
  * License along with this software; if not, write to the Free
  * Software Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA
  * 02110-1301 USA, or see the FSF site: http://www.fsf.org.
  */

using System;
using System.Runtime.InteropServices;

namespace PortAudioSharp {

	/**
		<summary>
			PortAudio v.19 bindings for .NET
		</summary>
	*/
	public class PortAudio
	{	
	    #region **** PORTAUDIO CALLBACKS ****
	    
	    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate PaStreamCallbackResult PaStreamCallbackDelegate(
	 		IntPtr input,
	 		IntPtr output,
	 		uint frameCount, 
	 		ref PaStreamCallbackTimeInfo timeInfo,
	 		PaStreamCallbackFlags statusFlags, 
	 		IntPtr userData);
	 		
	 	public delegate void PaStreamFinishedCallbackDelegate(IntPtr userData);
	 	
	 	#endregion
	 	
		#region **** PORTAUDIO DATA STRUCTURES ****
		
		[StructLayout (LayoutKind.Sequential)]
		public struct PaDeviceInfo {
			
			public int structVersion;
			[MarshalAs (UnmanagedType.LPStr)]
			public string name;
			public int hostApi;
			public int maxInputChannels;
			public int maxOutputChannels;
			public double defaultLowInputLatency;
			public double defaultLowOutputLatency;
			public double defaultHighInputLatency;
			public double defaultHighOutputLatency;
			public double defaultSampleRate;
			
			public override string ToString() {
				return "[" + this.GetType().Name + "]" + "\n"
					+ "name: " + name + "\n"
					+ "hostApi: " + hostApi + "\n"
					+ "maxInputChannels: " + maxInputChannels + "\n"
					+ "maxOutputChannels: " + maxOutputChannels + "\n"
					+ "defaultLowInputLatency: " + defaultLowInputLatency + "\n"
					+ "defaultLowOutputLatency: " + defaultLowOutputLatency + "\n"
					+ "defaultHighInputLatency: " + defaultHighInputLatency + "\n"
					+ "defaultHighOutputLatency: " + defaultHighOutputLatency + "\n"
					+ "defaultSampleRate: " + defaultSampleRate;
			}
		}
		
		[StructLayout (LayoutKind.Sequential)]
		public struct PaHostApiInfo {
			
			public int structVersion;
			public PaHostApiTypeId type;
			[MarshalAs (UnmanagedType.LPStr)]
			public string name;
			public int deviceCount;
			public int defaultInputDevice;
			public int defaultOutputDevice;
			
			public override string ToString() {
				return "[" + this.GetType().Name + "]" + "\n"
					+ "structVersion: " + structVersion + "\n"
					+ "type: " + type + "\n"
					+ "name: " + name + "\n"
					+ "deviceCount: " + deviceCount + "\n"
					+ "defaultInputDevice: " + defaultInputDevice + "\n"
					+ "defaultOutputDevice: " + defaultOutputDevice;
			}
		}
		
		[StructLayout (LayoutKind.Sequential)]
		public struct PaHostErrorInfo {
			
			public PaHostApiTypeId 	hostApiType;
			public int errorCode;
			[MarshalAs (UnmanagedType.LPStr)]
			public string errorText;
			
			public override string ToString() {
				return "[" + this.GetType().Name + "]" + "\n"
					+ "hostApiType: " + hostApiType + "\n"
					+ "errorCode: " + errorCode + "\n"
					+ "errorText: " + errorText;
			}
		}
		
		[StructLayout (LayoutKind.Sequential)]
		public struct PaStreamCallbackTimeInfo {
			
	 		public double currentTime;
  			public double inputBufferAdcTime;
  			public double outputBufferDacTime;
  			
  			public override string ToString() {
				return "[" + this.GetType().Name + "]" + "\n"
					+ "currentTime: " + currentTime + "\n"
					+ "inputBufferAdcTime: " + inputBufferAdcTime + "\n"
					+ "outputBufferDacTime: " + outputBufferDacTime;
			}
	 	}
	 	
	 	[StructLayout (LayoutKind.Sequential)]
		public struct PaStreamInfo {
	 		
			public int structVersion;
			public double inputLatency;
			public double outputLatency;
			public double sampleRate;
			
			public override string ToString() {
				return "[" + this.GetType().Name + "]" + "\n"
					+ "structVersion: " + structVersion + "\n"
					+ "inputLatency: " + inputLatency + "\n"
					+ "outputLatency: " + outputLatency + "\n"
					+ "sampleRate: " + sampleRate;
			}
		}
	 	
		[StructLayout (LayoutKind.Sequential)]
		public struct PaStreamParameters {
			
			public int device;
			public int channelCount;
			public PaSampleFormat sampleFormat;
			public double suggestedLatency;
			public IntPtr hostApiSpecificStreamInfo;
			
			public override string ToString() {
				return "[" + this.GetType().Name + "]" + "\n"
					+ "device: " + device + "\n"
					+ "channelCount: " + channelCount + "\n"
					+ "sampleFormat: " + sampleFormat + "\n"
					+ "suggestedLatency: " + suggestedLatency;
			}
		}
	 		
		#endregion
		
		#region **** PORTAUDIO DEFINES ****
		
		public enum PaDeviceIndex: int
		{
			paNoDevice = -1,
			paUseHostApiSpecificDeviceSpecification = -2
		}

		public enum PaSampleFormat: uint
		{
		   paFloat32 = 0x00000001,
		   paInt32 = 0x00000002,
		   paInt24 = 0x00000004,
		   paInt16 = 0x00000008,
		   paInt8 = 0x00000010,
		   paUInt8 = 0x00000020,
		   paCustomFormat = 0x00010000,
		   paNonInterleaved = 0x80000000,
		} 

		public const int paFormatIsSupported = 0;
		public const int paFramesPerBufferUnspecified = 0;
		
		public enum PaStreamFlags: uint
		{
			paNoFlag = 0,
			paClipOff = 0x00000001,
			paDitherOff = 0x00000002,
			paNeverDropInput = 0x00000004,
			paPrimeOutputBuffersUsingStreamCallback = 0x00000008,
			paPlatformSpecificFlags = 0xFFFF0000
		}
		
		public enum PaStreamCallbackFlags: uint
		{
			paInputUnderflow = 0x00000001,
			paInputOverflow = 0x00000002,
			paOutputUnderflow = 0x00000004,
			paOutputOverflow = 0x00000008,
			paPrimingOutput = 0x00000010
		}
		
		#endregion
		
		#region **** PORTAUDIO ENUMERATIONS ****
		
		public enum PaErrorCode : int {
  			paNoError = 0,
  			paNotInitialized = -10000,
  			paUnanticipatedHostError,
  			paInvalidChannelCount,
  			paInvalidSampleRate,
  			paInvalidDevice,
  			paInvalidFlag,
  			paSampleFormatNotSupported,
  			paBadIODeviceCombination, 
  			paInsufficientMemory, 
  			paBufferTooBig, 
  			paBufferTooSmall,
  			paNullCallback, 
  			paBadStreamPtr,
  			paTimedOut,
  			paInternalError,
  			paDeviceUnavailable, 
  			paIncompatibleHostApiSpecificStreamInfo, 
  			paStreamIsStopped, 
  			paStreamIsNotStopped,
  			paInputOverflowed, 
  			paOutputUnderflowed, 
  			paHostApiNotFound, 
  			paInvalidHostApi,
  			paCanNotReadFromACallbackStream, 
  			paCanNotWriteToACallbackStream, 
  			paCanNotReadFromAnOutputOnlyStream, 
  			paCanNotWriteToAnInputOnlyStream,
  			paIncompatibleStreamHostApi
  		}

		public enum PaHostApiTypeId : uint {
  			paInDevelopment = 0, 
  			paDirectSound = 1, 
  			paMME = 2, 
  			paASIO = 3,
  			paSoundManager = 4, 
  			paCoreAudio = 5, 
  			paOSS = 7, 
  			paALSA = 8,
  			paAL = 9, 
  			paBeOS = 10
		}
		
		public enum PaStreamCallbackResult : uint { 
			paContinue = 0, 
			paComplete = 1, 
			paAbort = 2 
		}
		
		#endregion
		
		#region **** PORTAUDIO FUNCTIONS ****
		
		[DllImport ("PortAudio.dll")]
	 	public static extern int Pa_GetVersion();
	 	
	 	[DllImport ("PortAudio.dll",EntryPoint="Pa_GetVersionText")]
	 	private static extern IntPtr IntPtr_Pa_GetVersionText();
	 	
	 	public static string Pa_GetVersionText() {
	 		IntPtr strptr = IntPtr_Pa_GetVersionText();
	 		return Marshal.PtrToStringAnsi(strptr);
	 	}
	 	
	 	[DllImport ("PortAudio.dll",EntryPoint="Pa_GetErrorText")]
	 	public static extern IntPtr IntPtr_Pa_GetErrorText(PaErrorCode errorCode);
	 	
	 	public static string Pa_GetErrorText(PaErrorCode errorCode) {
	 		IntPtr strptr = IntPtr_Pa_GetErrorText(errorCode);
	 		return Marshal.PtrToStringAnsi(strptr);
	 	}
	 	
	 	[DllImport ("PortAudio.dll")]
	 	public static extern PaErrorCode Pa_Initialize();
	 	
	 	[DllImport ("PortAudio.dll")]
	 	public static extern PaErrorCode Pa_Terminate();
	 	
		[DllImport ("PortAudio.dll")]
	 	public static extern int Pa_GetHostApiCount();

		[DllImport ("PortAudio.dll")]
	 	public static extern int Pa_GetDefaultHostApi();

		[DllImport ("PortAudio.dll",EntryPoint="Pa_GetHostApiInfo")]
	 	public static extern IntPtr IntPtr_Pa_GetHostApiInfo(int hostApi);
	 	
	 	public static PaHostApiInfo Pa_GetHostApiInfo(int hostApi) {
	 		IntPtr structptr = IntPtr_Pa_GetHostApiInfo(hostApi);
	 		return (PaHostApiInfo) Marshal.PtrToStructure(structptr,typeof(PaHostApiInfo));
	 	}
		
		[DllImport ("PortAudio.dll")]
	 	public static extern int Pa_HostApiTypeIdToHostApiIndex(PaHostApiTypeId type);

		[DllImport ("PortAudio.dll")]
	 	public static extern int Pa_HostApiDeviceIndexToDeviceIndex(int hostApi, int hostApiDeviceIndex);

		[DllImport ("PortAudio.dll",EntryPoint="Pa_GetLastHostErrorInfo")]
	 	public static extern IntPtr IntPtr_Pa_GetLastHostErrorInfo();
	 	
	 	public static PaHostErrorInfo Pa_GetLastHostErrorInfo() {
	 		IntPtr structptr = IntPtr_Pa_GetLastHostErrorInfo();
	 		return (PaHostErrorInfo) Marshal.PtrToStructure(structptr,typeof(PaHostErrorInfo));
	 	}

		[DllImport ("PortAudio.dll")]
	 	public static extern int Pa_GetDeviceCount();
		
		[DllImport ("PortAudio.dll")]
	 	public static extern int Pa_GetDefaultInputDevice();

		[DllImport ("PortAudio.dll")]
	 	public static extern int Pa_GetDefaultOutputDevice();
		
		[DllImport ("PortAudio.dll",EntryPoint="Pa_GetDeviceInfo")]
	 	public static extern IntPtr IntPtr_Pa_GetDeviceInfo(int device);
	 	
	 	public static PaDeviceInfo Pa_GetDeviceInfo(int device) {
	 		IntPtr structptr = IntPtr_Pa_GetDeviceInfo(device);
	 		return (PaDeviceInfo) Marshal.PtrToStructure(structptr,typeof(PaDeviceInfo));
	 	}
		
		[DllImport ("PortAudio.dll")]
	 	public static extern PaErrorCode Pa_IsFormatSupported(
	 		ref PaStreamParameters inputParameters, 
	 		ref PaStreamParameters outputParameters, 
	 		double sampleRate);
		
		[DllImport ("PortAudio.dll")]
	 	public static extern PaErrorCode Pa_OpenStream(
	 		out IntPtr stream,
	 		ref PaStreamParameters inputParameters, 
	 		ref PaStreamParameters outputParameters,
	 		double sampleRate, 
	 		uint framesPerBuffer,
	 		PaStreamFlags streamFlags,
	 		PaStreamCallbackDelegate streamCallback,
	 		IntPtr userData);

		[DllImport ("PortAudio.dll")]
	 	public static extern PaErrorCode Pa_OpenDefaultStream(
	 		out IntPtr stream,
	 		int numInputChannels, 
	 		int numOutputChannels, 
	 		uint sampleFormat,
	 		double sampleRate, 
	 		uint framesPerBuffer,
	 		PaStreamCallbackDelegate streamCallback,
	 		IntPtr userData);
	 		
		[DllImport ("PortAudio.dll")]
	 	public static extern PaErrorCode Pa_CloseStream(IntPtr stream);
	 	
		[DllImport ("PortAudio.dll")]
	 	public static extern PaErrorCode Pa_SetStreamFinishedCallback(
	 		ref IntPtr stream,
	 		[MarshalAs(UnmanagedType.FunctionPtr)]PaStreamFinishedCallbackDelegate streamFinishedCallback);
		
		[DllImport ("PortAudio.dll")]
	 	public static extern PaErrorCode Pa_StartStream(IntPtr stream);
		
		[DllImport ("PortAudio.dll")]
	 	public static extern PaErrorCode Pa_StopStream(IntPtr stream);
		
		[DllImport ("PortAudio.dll")]
	 	public static extern PaErrorCode Pa_AbortStream(IntPtr stream);
		
		[DllImport ("PortAudio.dll")]
	 	public static extern PaErrorCode Pa_IsStreamStopped(IntPtr stream);
		
		[DllImport ("PortAudio.dll")]
	 	public static extern PaErrorCode Pa_IsStreamActive(IntPtr stream);
		
		[DllImport ("PortAudio.dll",EntryPoint="Pa_GetStreamInfo")]
	 	public static extern IntPtr IntPtr_Pa_GetStreamInfo(IntPtr stream);
	 	
	 	public static PaStreamInfo Pa_GetStreamInfo(IntPtr stream) {
	 		IntPtr structptr = IntPtr_Pa_GetStreamInfo(stream);
	 		return (PaStreamInfo) Marshal.PtrToStructure(structptr,typeof(PaStreamInfo));
	 	}
		
		[DllImport ("PortAudio.dll")]
	 	public static extern double Pa_GetStreamTime(IntPtr stream);
		
		[DllImport ("PortAudio.dll")]
	 	public static extern double Pa_GetStreamCpuLoad(IntPtr stream);
		
		[DllImport ("PortAudio.dll")]
	 	public static extern PaErrorCode Pa_ReadStream(
	 		IntPtr stream,
	 		[Out]float[] buffer,
			uint frames);
		
		[DllImport ("PortAudio.dll")]
	 	public static extern PaErrorCode Pa_WriteStream(
	 		IntPtr stream,
	 		[In]float[] buffer,
			uint frames);
		
		[DllImport ("PortAudio.dll")]
	 	public static extern int Pa_GetStreamReadAvailable(IntPtr stream);
		
		[DllImport ("PortAudio.dll")]
	 	public static extern int Pa_GetStreamWriteAvailable(IntPtr stream);
		
		[DllImport ("PortAudio.dll")]
	 	public static extern PaErrorCode Pa_GetSampleSize(PaSampleFormat format);
		
		[DllImport ("PortAudio.dll")]
	 	public static extern void Pa_Sleep(int msec);
	 	
	 	#endregion
	 	
	 	private PortAudio() {
	 		// This is a static class
	 	}
	 	
	 }
 
 }
