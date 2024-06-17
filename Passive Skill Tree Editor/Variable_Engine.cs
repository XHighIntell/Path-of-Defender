using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace PassiveSkillScreen_Editor
{
    public static class Variable_Engine {
        public static byte[] StructToByte(object iStructure) {
            byte[] buffer = new byte[Marshal.SizeOf(iStructure)];
            GCHandle h = GCHandle.Alloc(buffer, GCHandleType.Pinned); ;
            Marshal.StructureToPtr(iStructure, h.AddrOfPinnedObject(), false);
            h.Free(); return buffer;
        }

        public static object ByteToStruct(byte[] iByte, Type iStructureType) {
            object TMP_object;
            GCHandle handle = GCHandle.Alloc(iByte, GCHandleType.Pinned);
            TMP_object = (object)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), iStructureType);
            handle.Free(); return TMP_object;
        }
    }
}
