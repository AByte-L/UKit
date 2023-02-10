using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace AByte.UKit.Utilities
{
    public static class NodesHelper
    {
        private IEnumerable<T> GetNodesByCondtion(List<T> nodes, Func<T, List<T>> GetChilds,  Func<T, bool> predicate)
        {
            foreach (var item in nodes)
            {
                if (ipredicate(item))
                {
                    yield return item.Name;
                }
                else
                {
                    var subList = GetNodesByCondtion(item.Childs,predicate);
                    foreach (var sub in subList)
                    {
                        yield return sub;
                    }
                }
            }
        }

    }
}


