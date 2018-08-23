using System;
using System.Collections.Generic;
using BovineLabs.Common;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;

namespace BovineLabs.Part1
{
    public static class ListExtensions
    {
        public static  unsafe void NativeAddRange<T>(this List<T> list, NativeArray<T> array) where T : struct
        {
            NativeAddRange(list, array.GetUnsafePtr(), array.Length);
        }

        public static unsafe void NativeAddRange<T>(this List<T> list, NativeList<T> nativeList) where T : struct
        {
            NativeAddRange(list, nativeList.GetUnsafePtr(), nativeList.Length);
        }
        
        public static unsafe void NativeAddRange<T>(this List<T> list, NativeSlice<T> nativeSlice) where T : struct
        {          
            NativeAddRange(list, nativeSlice.GetUnsafePtr(), nativeSlice.Length);
        }
        
        public static unsafe void NativeAddRange<T>(this List<T> list, DynamicBuffer<T> buffer) where T : struct
        {          
            NativeAddRange(list, buffer.GetBasePointer(), buffer.Length);
        }

        private static unsafe void NativeAddRange<T>(List<T> list, void* arrayBuffer, int length)
            where T : struct
        {
            var index = list.Count;
            var newLength = index + length;
            
            // Resize our list if we require
            if (list.Capacity < newLength)
                list.Capacity = newLength;

            var items = NoAllocHelpers.ExtractArrayFromListT(list);
            var size = UnsafeUtility.SizeOf<T>();

            // Get the pointer to the end of the list
            var bufferStart = (IntPtr) UnsafeUtility.AddressOf(ref items[0]);
            var buffer = (byte*) (bufferStart + size * index);

            UnsafeUtility.MemCpy(buffer, arrayBuffer, length * (long) size);

            NoAllocHelpers.ResizeList(list, newLength);
        }
    }
}