﻿using System.Collections.Generic;
using ChatBotFramework.Account.Interface;
using ChatBotFramework.Network.Interface;

namespace ChatBotFramework.Network {
    class AsyncNetworkManager {
        private readonly Dictionary<IChatBotAccount, IAsyncNetworkClient> AccountClientMap = new Dictionary<IChatBotAccount, IAsyncNetworkClient>();
        public TypeReceiveCallback Dispatcher { private get; set; }

        public void BindAccountWithClient(IChatBotAccount Account, IAsyncNetworkClient Client) {
            if (!this.AccountClientMap.ContainsKey(Account)) {
                lock (this.AccountClientMap) {
                    this.AccountClientMap.Add(Account, Client);
                    this.AccountClientMap[Account].ReceiveCallBack = this.Dispatcher;
                    this.AccountClientMap[Account].Begin(); 
                }
            } else {
                lock (this.AccountClientMap) {
                    this.AccountClientMap[Account].Stop();
                    this.AccountClientMap[Account] = Client;
                    this.AccountClientMap[Account].ReceiveCallBack = this.Dispatcher;
                    this.AccountClientMap[Account].Begin();
                }
            }
        }

        public void RemoveAccount(IChatBotAccount Account) {
            if (this.AccountClientMap.ContainsKey(Account)) {
                lock (this.AccountClientMap) {
                    this.AccountClientMap[Account].Stop();
                    this.AccountClientMap.Remove(Account);
                }
            }
        }
    }
}